from __future__ import annotations

from collections import OrderedDict
from dataclasses import dataclass
from pathlib import Path

import openpyxl


ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / "docs" / "specs" / "DOR_Agile_Backlog_User_Stories.xlsx"
TARGET = ROOT / "docs" / "specs" / "DOR_Agile_Backlog_User_Stories.md"
SHEET_NAME = "Agile Backlog"
HEADER_ROW = 4
GENERATED_ON = "2026-09-03"


@dataclass(frozen=True)
class Phase:
    name: str
    title: str
    epics: tuple[str, ...]
    sprint_candidates: tuple[str, ...]
    planning_note: str


PHASES: tuple[Phase, ...] = (
    Phase(
        "Phase 1",
        "Core Work Intake, Collaboration, and Organization",
        ("E1", "E2"),
        (
            "Collaboration workspace, notifications, task creation, assignment, and work queues",
            "Identifiers, cross-record relationships, packages, sorting, filtering, and grouping",
        ),
        "Builds the core operating model that later workflow, documents, reporting, and integrations depend on.",
    ),
    Phase(
        "Phase 2",
        "Workflow, Review, Authoring, and Document Production",
        ("E3", "E4"),
        (
            "Configurable workflow, multi-reviewer routing, executive review, priorities, and due dates",
            "Rich-text authoring, attachments, work-in-progress save, templates, generated documents, and reuse",
        ),
        "Turns the intake model into usable legislative work production with review and approval controls.",
    ),
    Phase(
        "Phase 3",
        "Legislative Data Lifecycle, Search, and Reporting",
        ("E5", "E6"),
        (
            "External legislative updates, bill status updates, amendments, version retention, and comparison",
            "Enterprise search, predetermined reports, saved queries, database query access, and extracts",
        ),
        "Adds the bill lifecycle data, version history, and find/report capabilities needed for scale.",
    ),
    Phase(
        "Phase 4",
        "Fiscal Analysis, Financial Inputs, and Productivity Integration",
        ("E7", "E8", "E14"),
        (
            "Fiscal data retrieval, fiscal-work documentation, budget reconciliation, demographics, and expense estimates",
            "Microsoft 365 integration for Teams, Outlook, Excel, and Word productivity workflows",
        ),
        "Connects legislative work to fiscal analysis, budget inputs, and productivity tools.",
    ),
    Phase(
        "Phase 5",
        "Security, Operations, Migration, and Historical Reference",
        ("E9", "E10"),
        (
            "Role-based security, partial work-product restrictions, concurrency, and availability",
            "Legacy migration and 10-year historical fiscal reference access",
        ),
        "Hardens the solution for enterprise operation and preserves historical context.",
    ),
    Phase(
        "Phase 6",
        "Specialized Legislative Programs and Executive Experience",
        ("E11", "E12", "E13"),
        (
            "L&P correspondence and legislative implementation management",
            "Remote/mobile executive access, executive bill view, and executive bill-page discussion",
        ),
        "Layers specialized workflows and executive experiences on top of the established system capabilities.",
    ),
)


def clean(value: object) -> str:
    if value is None:
        return ""
    return " ".join(str(value).replace("\r", "\n").split())


def bullet_text(value: str) -> str:
    value = clean(value)
    return value if value else "None recorded."


def read_rows() -> list[dict[str, str]]:
    workbook = openpyxl.load_workbook(SOURCE, data_only=True)
    worksheet = workbook[SHEET_NAME]
    headers = [clean(cell.value) for cell in worksheet[HEADER_ROW]]
    rows: list[dict[str, str]] = []

    for worksheet_row in worksheet.iter_rows(min_row=HEADER_ROW + 1, values_only=True):
        if not any(value is not None for value in worksheet_row):
            continue
        row = {header: clean(value) for header, value in zip(headers, worksheet_row)}
        rows.append(row)

    return rows


def group_ordered(rows: list[dict[str, str]], key_fields: tuple[str, ...]) -> OrderedDict[tuple[str, ...], list[dict[str, str]]]:
    groups: OrderedDict[tuple[str, ...], list[dict[str, str]]] = OrderedDict()
    for row in rows:
        key = tuple(row[field] for field in key_fields)
        groups.setdefault(key, []).append(row)
    return groups


def priority_counts(rows: list[dict[str, str]]) -> str:
    must = sum(1 for row in rows if row["Priority"] == "Must Have")
    nice = sum(1 for row in rows if row["Priority"] == "Nice to Have")
    return f"{must} must-have, {nice} nice-to-have, {len(rows)} total"


def acceptance_criteria(row: dict[str, str]) -> list[str]:
    return [
        row[f"Acceptance Criterion {index}"]
        for index in range(1, 7)
        if row.get(f"Acceptance Criterion {index}")
    ]


def phase_for_epic(epic_id: str) -> Phase:
    for phase in PHASES:
        if epic_id in phase.epics:
            return phase
    raise ValueError(f"No phase configured for epic {epic_id}")


def write_markdown(rows: list[dict[str, str]]) -> None:
    lines: list[str] = []

    lines.extend(
        [
            "# DOR Agile Backlog User Stories",
            "",
            f"Generated: {GENERATED_ON}",
            "",
            f"Source workbook: `{SOURCE.relative_to(ROOT).as_posix()}`",
            "",
            f"Source worksheet: `{SHEET_NAME}`",
            "",
            "Purpose: This Markdown file converts the business agile backlog into a sprint-planning-ready reference. It preserves story-level traceability, acceptance criteria, priority, open questions, and source-document references from the source workbook.",
            "",
            "Planning status: This document is a derivative planning resource. It does not authorize implementation by itself; implementation remains governed by `docs/02-Current-Sprint.md`.",
            "",
            "Regeneration: Run `python tools/convert_dor_backlog.py` from the repository root after the source workbook changes.",
            "",
            "## Conversion Summary",
            "",
            f"- Total user stories: {len(rows)}",
            f"- Priority mix: {priority_counts(rows)}",
            "- Organization: stories are grouped into logical phases, then epics, features, and user stories.",
            "- Sprint use: each phase includes candidate sprint planning units, but final sprint scope should be promoted into `docs/02-Current-Sprint.md` before implementation.",
            "",
            "## Logical Phase Map",
            "",
            "| Phase | Focus | Epics | Candidate Sprint Planning Units | Story Count |",
            "| --- | --- | --- | --- | ---: |",
        ]
    )

    for phase in PHASES:
        phase_rows = [row for row in rows if row["Epic ID"] in phase.epics]
        candidates = "<br>".join(phase.sprint_candidates)
        lines.append(
            f"| {phase.name} | {phase.title} | {', '.join(phase.epics)} | {candidates} | {len(phase_rows)} |"
        )

    lines.append("")

    for phase in PHASES:
        phase_rows = [row for row in rows if row["Epic ID"] in phase.epics]
        lines.extend(
            [
                f"## {phase.name} - {phase.title}",
                "",
                phase.planning_note,
                "",
                f"Phase backlog: {priority_counts(phase_rows)}.",
                "",
                "Candidate sprint planning units:",
            ]
        )
        for candidate in phase.sprint_candidates:
            lines.append(f"- {candidate}")
        lines.append("")

        epics = group_ordered(phase_rows, ("Epic ID", "Epic"))
        for (epic_id, epic), epic_rows in epics.items():
            lines.extend(
                [
                    f"### {epic_id} - {epic}",
                    "",
                    f"Epic backlog: {priority_counts(epic_rows)}.",
                    "",
                ]
            )

            features = group_ordered(epic_rows, ("Feature ID", "Feature"))
            for (feature_id, feature), feature_rows in features.items():
                lines.extend(
                    [
                        f"#### {feature_id} - {feature}",
                        "",
                        f"Feature backlog: {priority_counts(feature_rows)}.",
                        "",
                    ]
                )

                for row in feature_rows:
                    lines.extend(
                        [
                            f"##### {row['User Story ID']} - {row['Source Requirement']}",
                            "",
                            f"- Priority: {row['Priority']}",
                            f"- User role: {row['User Role']}",
                            f"- User story: {row['User Story']}",
                            "- Acceptance criteria:",
                        ]
                    )

                    criteria = acceptance_criteria(row)
                    if criteria:
                        for index, criterion in enumerate(criteria, start=1):
                            lines.append(f"  {index}. {criterion}")
                    else:
                        lines.append("  1. None recorded.")

                    lines.extend(
                        [
                            f"- Open question / clarification: {bullet_text(row['Open Question / Clarification'])}",
                            f"- Source document: {row['Source Document']}",
                            "",
                        ]
                    )

    lines.extend(
        [
            "## Source Trace Matrix",
            "",
            "| User Story ID | Source Requirement | Priority | Phase | Epic | Feature |",
            "| --- | --- | --- | --- | --- | --- |",
        ]
    )

    for row in rows:
        phase = phase_for_epic(row["Epic ID"])
        lines.append(
            f"| {row['User Story ID']} | {row['Source Requirement']} | {row['Priority']} | {phase.name} | {row['Epic ID']} - {row['Epic']} | {row['Feature ID']} - {row['Feature']} |"
        )

    lines.append("")
    TARGET.write_text("\n".join(lines), encoding="utf-8", newline="\n")


def main() -> None:
    rows = read_rows()
    if len(rows) != 69:
        raise RuntimeError(f"Expected 69 backlog rows, found {len(rows)}")

    missing_phase = sorted({row["Epic ID"] for row in rows} - {epic for phase in PHASES for epic in phase.epics})
    if missing_phase:
        raise RuntimeError(f"Missing phase mapping for epics: {', '.join(missing_phase)}")

    write_markdown(rows)
    print(f"Wrote {TARGET.relative_to(ROOT)} with {len(rows)} stories across {len(PHASES)} logical phases.")


if __name__ == "__main__":
    main()
