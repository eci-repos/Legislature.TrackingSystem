from __future__ import annotations

from collections import OrderedDict
from dataclasses import dataclass
from pathlib import Path
import re

import openpyxl


ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / "docs" / "specs" / "DOR_Agile_Backlog_User_Stories.xlsx"
BACKLOG_DIR = ROOT / "docs" / "backlog"
SHEET_NAME = "Agile Backlog"
HEADER_ROW = 4
GENERATED_ON = "2026-09-04"


@dataclass(frozen=True)
class Phase:
    number: int
    title: str
    epics: tuple[str, ...]
    sprint_path: str
    objective: str

    @property
    def label(self) -> str:
        return f"Phase {self.number}"

    @property
    def file_name(self) -> str:
        slug = re.sub(r"[^a-z0-9]+", "-", self.title.lower()).strip("-")
        return f"phase-{self.number:02d}-{slug}.md"


PHASES: tuple[Phase, ...] = (
    Phase(
        1,
        "Core Work Intake, Collaboration, and Organization",
        ("E1", "E2"),
        "POC foundation through POC acceptance",
        "Create the smallest useful LTS vertical slice: intake, identifiers, assignments, work queues, relationships, packages, filtering, grouping, and traceability.",
    ),
    Phase(
        2,
        "Workflow, Review, Authoring, and Document Production",
        ("E3", "E4"),
        "Post-POC implementation expansion",
        "Add configurable workflow, review, approval, rich-text authoring, attachments, templates, generated documents, and reuse.",
    ),
    Phase(
        3,
        "Legislative Data Lifecycle, Search, and Reporting",
        ("E5", "E6"),
        "Post-POC implementation expansion",
        "Add external legislative update boundaries, version history, comparison, search, reports, custom queries, and extracts.",
    ),
    Phase(
        4,
        "Fiscal Analysis, Financial Inputs, and Productivity Integration",
        ("E7", "E8", "E14"),
        "Post-POC implementation expansion",
        "Add fiscal-data boundaries, supporting fiscal analysis, budget-office inputs, and Microsoft 365 productivity integration.",
    ),
    Phase(
        5,
        "Security, Operations, Migration, and Historical Reference",
        ("E9", "E10"),
        "Enterprise hardening and transition",
        "Harden role restrictions, performance, support availability, migration, and historical reference capabilities.",
    ),
    Phase(
        6,
        "Specialized Legislative Programs and Executive Experience",
        ("E11", "E12", "E13"),
        "Specialized capability expansion",
        "Add L&P correspondence, legislative implementation management, remote/mobile access, executive bill view, and executive discussion.",
    ),
)


TECHNICAL_BASELINE = (
    "ASP.NET Core / Blazor WebAssembly POC stack",
    "Modular monolith with Domain, Application, Infrastructure, Web, and Tests boundaries",
    "Versioned API and UI behavior traceable to source story IDs",
    "Unit/API/component tests for promoted acceptance criteria",
    "Documentation updates in sprint, handoff, and traceability records",
    "PM Validation report after completed and verified sprint and/or promoted use case scope",
)


FEATURE_NOTES: dict[str, tuple[str, ...]] = {
    "F1.1": ("Model a collaboration workspace or common record context before adding comments, notifications, or documents.",),
    "F1.2": ("Start with in-app notification events in the POC; defer delivery channels until requirements are clarified.",),
    "F1.3": ("Define mandatory task fields before implementation; include due date, priority, status, owner, and trace references.",),
    "F1.4": ("Preserve change intent and audit fields when tasks are updated, canceled, duplicated, corrected, or reopened.",),
    "F2.1": ("Make identifier semantics explicit and testable; do not infer item type from display text.",),
    "F2.2": ("Represent relationships as first-class links with type, source item, target item, and audit metadata.",),
    "F2.3": ("Use controlled criteria for sorting, filtering, and grouping; avoid free-form categories where enums are required.",),
    "F3.1": ("Keep workflow states explicit; separate preparation and approval responsibilities.",),
    "F3.2": ("Treat Executive Review as a governed workflow path with role-specific permissions.",),
    "F4.1": ("Defer full editor selection until requirements are promoted; define content contracts first.",),
    "F4.2": ("Store document metadata separately from binary/document repository concerns.",),
    "F4.3": ("Treat template merge fields as explicit data contracts.",),
    "F4.4": ("Copy or transfer prior work through structured reuse operations, not manual paste assumptions.",),
    "F5.1": ("Keep legislative-source ingestion behind adapters until current external contracts are confirmed.",),
    "F5.2": ("Preserve immutable version snapshots and comparison boundaries.",),
    "F6.1": ("Design search around authorized visibility and indexed record metadata.",),
    "F6.2": ("Separate standard reports, saved queries, governed database analysis, and external extracts.",),
    "F7.1": ("Keep fiscal-system calls behind adapters and use synthetic data for development.",),
    "F7.2": ("Model budget and demographic data as versioned session-specific references.",),
    "F8.1": ("Use Microsoft 365 integration boundaries first; real tenant integration requires later configuration.",),
    "F9.1": ("Enforce least-privilege policies in application services and UI visibility.",),
    "F9.2": ("Add performance and availability tests when implementation scope exists.",),
    "F10.1": ("Treat migration as a repeatable import with validation, reconciliation, and rollback records.",),
    "F10.2": ("Model historical access around years, sessions, and fiscal work-product references.",),
    "F11.1": ("Capture correspondence recipient, sent state, response state, and linked bill/work context.",),
    "F12.1": ("Keep implementation tasks distinct from pre-enactment legislative work tasks.",),
    "F13.1": ("Design responsive access and identity boundaries before mobile-specific refinements.",),
    "F13.2": ("Make executive bill view read-optimized and source-linked.",),
    "F14.1": ("Model expense estimate inputs as auditable fiscal assumptions.",),
}


def clean(value: object) -> str:
    if value is None:
        return ""
    return " ".join(str(value).replace("\r", "\n").split())


def read_rows() -> list[dict[str, str]]:
    workbook = openpyxl.load_workbook(SOURCE, data_only=True)
    worksheet = workbook[SHEET_NAME]
    headers = [clean(cell.value) for cell in worksheet[HEADER_ROW]]
    rows: list[dict[str, str]] = []

    for worksheet_row in worksheet.iter_rows(min_row=HEADER_ROW + 1, values_only=True):
        if not any(value is not None for value in worksheet_row):
            continue
        rows.append({header: clean(value) for header, value in zip(headers, worksheet_row)})

    return rows


def group_ordered(rows: list[dict[str, str]], key_fields: tuple[str, ...]) -> OrderedDict[tuple[str, ...], list[dict[str, str]]]:
    grouped: OrderedDict[tuple[str, ...], list[dict[str, str]]] = OrderedDict()
    for row in rows:
        key = tuple(row[field] for field in key_fields)
        grouped.setdefault(key, []).append(row)
    return grouped


def criteria(row: dict[str, str]) -> list[str]:
    return [row[f"Acceptance Criterion {index}"] for index in range(1, 7) if row.get(f"Acceptance Criterion {index}")]


def phase_for(row: dict[str, str]) -> Phase:
    for phase in PHASES:
        if row["Epic ID"] in phase.epics:
            return phase
    raise ValueError(f"No phase for epic {row['Epic ID']}")


def priority_counts(rows: list[dict[str, str]]) -> str:
    must = sum(1 for row in rows if row["Priority"] == "Must Have")
    nice = sum(1 for row in rows if row["Priority"] == "Nice to Have")
    return f"{must} must-have, {nice} nice-to-have, {len(rows)} total"


def backlog_id(row: dict[str, str]) -> str:
    return f"BI-{row['User Story ID']}"


def write_readme(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Backlog",
        "",
        "Status: active planning backlog",
        "",
        f"Last updated: {GENERATED_ON}",
        "",
        "## Purpose",
        "",
        "This folder contains proposed backlog items prepared from the DOR business agile backlog and related project planning documents. These files help define future sprints, but they do not authorize implementation by themselves. Implementation remains governed by `docs/02-Current-Sprint.md`.",
        "",
        "## Source",
        "",
        f"- Source workbook: `docs/specs/{SOURCE.name}`",
        f"- Source worksheet: `{SHEET_NAME}`",
        "- Generated planning reference: `docs/specs/DOR_Agile_Backlog_User_Stories.md`",
        "- Recommended stack reference: `docs/04-Recommended-Tech-Stack.md`",
        "- Pair schedule reference: `docs/05-Pair-WorkPlan-and-Schedule.md`",
        "- Timeline reference: `docs/06-Companion-Timeline.md`",
        "",
        "## Backlog Index",
        "",
        "| Phase | Backlog File | Focus | Story Count | Development Path |",
        "| --- | --- | --- | ---: | --- |",
    ]

    for phase in PHASES:
        phase_rows = [row for row in rows if row["Epic ID"] in phase.epics]
        lines.append(
            f"| {phase.label} | `{phase.file_name}` | {phase.title} | {len(phase_rows)} | {phase.sprint_path} |"
        )

    lines.extend(
        [
            "",
            "## Planning Rules",
            "",
            "- Backlog items are proposed until promoted into `docs/02-Current-Sprint.md`.",
            "- Each item keeps its source user story ID and source requirement ID.",
            "- Sprint planning should pull the smallest coherent vertical slice that can be built, tested, reviewed, and documented.",
            "- Open questions must be resolved by the senior developer or stakeholder before being encoded as business rules.",
            "- A PM Validation report must be prepared after each sprint and/or promoted use case is completed and verified.",
            "- Fully implemented backlog files or items should be moved to `docs/backlog/archive/` only after the current sprint records completion and the PM Validation report is prepared.",
            "",
            "## Logical Next Steps",
            "",
            "1. Convert the technical agile backlog into Markdown for technical traceability.",
            "2. Create a Phase 1 POC sprint scope from `phase-01-core-work-intake-collaboration-and-organization.md`.",
            "3. Build a traceability matrix linking business backlog items to technical requirements, modules, and tests.",
            "4. Promote the first implementation slice into `docs/02-Current-Sprint.md` before scaffolding code.",
            "5. Prepare the PM Validation report after completed and verified sprint and/or promoted use case scope.",
            "6. Archive completed backlog items only after implementation, verification, documentation, and PM Validation reporting are complete.",
            "",
        ]
    )

    (BACKLOG_DIR / "README.md").write_text("\n".join(lines), encoding="utf-8", newline="\n")


def write_phase_file(phase: Phase, rows: list[dict[str, str]]) -> None:
    phase_rows = [row for row in rows if row["Epic ID"] in phase.epics]
    lines = [
        f"# Backlog - {phase.label}: {phase.title}",
        "",
        "Status: proposed",
        "",
        f"Last updated: {GENERATED_ON}",
        "",
        "## Planning Objective",
        "",
        phase.objective,
        "",
        f"Backlog size: {priority_counts(phase_rows)}.",
        "",
        f"Development path: {phase.sprint_path}.",
        "",
        "## Technical Baseline",
        "",
    ]

    for item in TECHNICAL_BASELINE:
        lines.append(f"- {item}")

    lines.extend(
        [
            "",
            "## Backlog Items",
            "",
        ]
    )

    for (epic_id, epic), epic_rows in group_ordered(phase_rows, ("Epic ID", "Epic")).items():
        lines.extend([f"### {epic_id} - {epic}", ""])

        for (feature_id, feature), feature_rows in group_ordered(epic_rows, ("Feature ID", "Feature")).items():
            lines.extend([f"#### {feature_id} - {feature}", ""])

            for row in feature_rows:
                lines.extend(
                    [
                        f"##### {backlog_id(row)}",
                        "",
                        f"- Source user story: {row['User Story ID']}",
                        f"- Source requirement: {row['Source Requirement']}",
                        f"- Priority: {row['Priority']}",
                        f"- User role: {row['User Role']}",
                        f"- Planning status: Proposed; not authorized until promoted into `docs/02-Current-Sprint.md`.",
                        f"- User story: {row['User Story']}",
                        "- Acceptance criteria:",
                    ]
                )

                item_criteria = criteria(row)
                if item_criteria:
                    for index, criterion in enumerate(item_criteria, start=1):
                        lines.append(f"  {index}. {criterion}")
                else:
                    lines.append("  1. None recorded.")

                open_question = row["Open Question / Clarification"] or "None recorded."
                lines.extend(
                    [
                        f"- Open question / clarification: {open_question}",
                        "- Development notes:",
                    ]
                )
                for note in FEATURE_NOTES.get(row["Feature ID"], ("Implement behind source-traceable application services and tests.",)):
                    lines.append(f"  - {note}")
                lines.extend(
                    [
                        f"  - Add or update tests that assert the promoted acceptance criteria for `{row['User Story ID']}`.",
                        f"  - Preserve traceability to `{row['Source Requirement']}` in implementation notes, tests, seed data, or UI/API metadata as appropriate.",
                        f"- Source document: {row['Source Document']}",
                        "",
                    ]
                )

    lines.extend(
        [
            "## Phase Trace Matrix",
            "",
            "| Backlog Item | User Story | Source Requirement | Priority | Feature |",
            "| --- | --- | --- | --- | --- |",
        ]
    )

    for row in phase_rows:
        lines.append(
            f"| {backlog_id(row)} | {row['User Story ID']} | {row['Source Requirement']} | {row['Priority']} | {row['Feature ID']} - {row['Feature']} |"
        )

    lines.append("")
    (BACKLOG_DIR / phase.file_name).write_text("\n".join(lines), encoding="utf-8", newline="\n")


def main() -> None:
    rows = read_rows()
    if len(rows) != 69:
        raise RuntimeError(f"Expected 69 business backlog rows, found {len(rows)}")

    configured_epics = {epic for phase in PHASES for epic in phase.epics}
    missing_epics = sorted({row["Epic ID"] for row in rows} - configured_epics)
    if missing_epics:
        raise RuntimeError(f"Missing phase mapping for epics: {', '.join(missing_epics)}")

    BACKLOG_DIR.mkdir(parents=True, exist_ok=True)
    write_readme(rows)
    for phase in PHASES:
        write_phase_file(phase, rows)

    print(f"Wrote backlog README and {len(PHASES)} phase files with {len(rows)} source stories.")


if __name__ == "__main__":
    main()
