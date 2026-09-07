from __future__ import annotations

from collections import OrderedDict
from dataclasses import dataclass
from pathlib import Path

import openpyxl


ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / "docs" / "specs" / "DOR_Technical_Agile_Backlog_User_Stories.xlsx"
OUTPUT = ROOT / "docs" / "specs" / "DOR_Technical_Agile_Backlog_User_Stories.md"
SHEET_NAME = "Technical Agile Backlog"
HEADER_ROW = 4
GENERATED_ON = "2026-09-04"
EXPECTED_STORY_COUNT = 94


@dataclass(frozen=True)
class TechnicalStory:
    epic_id: str
    epic: str
    feature: str
    story_id: str
    source_requirement: str
    requirement_type: str
    user_role: str
    user_story: str
    acceptance_criteria: tuple[str, ...]
    open_question: str
    conditional_applicability: str
    source_section: str
    source_page: str
    source_document: str


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


def to_story(row: dict[str, str]) -> TechnicalStory:
    criteria = tuple(row[f"Acceptance Criterion {index}"] for index in range(1, 8) if row.get(f"Acceptance Criterion {index}"))

    return TechnicalStory(
        epic_id=row["Epic #"],
        epic=row["Epic"],
        feature=row["Feature"],
        story_id=row["User Story ID"],
        source_requirement=row["Source Requirement"],
        requirement_type=row["Requirement Type"],
        user_role=row["User Role"],
        user_story=row["User Story"],
        acceptance_criteria=criteria,
        open_question=row["Open Question / Clarification"] or "None recorded.",
        conditional_applicability=row["Conditional Applicability"] or "Always applicable unless narrowed by a future promoted sprint.",
        source_section=row["Source Section"],
        source_page=row["Source Page"],
        source_document=row["Source Document"],
    )


def group_ordered(stories: list[TechnicalStory], key_selector) -> OrderedDict[str, list[TechnicalStory]]:
    grouped: OrderedDict[str, list[TechnicalStory]] = OrderedDict()
    for story in stories:
        grouped.setdefault(key_selector(story), []).append(story)
    return grouped


def write_markdown(stories: list[TechnicalStory]) -> None:
    lines = [
        "# DOR Technical Agile Backlog User Stories",
        "",
        f"Generated on: {GENERATED_ON}",
        "",
        f"Source workbook: `docs/specs/{SOURCE.name}`",
        "",
        f"Source worksheet: `{SHEET_NAME}`",
        "",
        "Purpose: This Markdown file converts the technical agile backlog into a sprint-planning-ready reference. It preserves technical story IDs, source requirement IDs, requirement types, acceptance criteria, conditional applicability, open questions, and source-document references from the source workbook.",
        "",
        "Authority: The original workbook remains the source file. This Markdown file is a generated planning aid and MUST NOT replace the original `.xlsx` source.",
        "",
        "Regeneration: Run `python tools/convert_dor_technical_backlog.py` from the repository root after the source workbook changes.",
        "",
        "## Summary",
        "",
        f"- Technical stories converted: {len(stories)}",
        f"- Technical epics converted: {len(group_ordered(stories, lambda story: story.epic_id))}",
        "- Sprint use: promote final implementation scope into `docs/02-Current-Sprint.md` before coding.",
        "- Traceability: link implementation, tests, configuration, and documentation back to technical story IDs and source requirement IDs.",
        "",
        "## Stories by Epic",
        "",
    ]

    for epic_key, epic_stories in group_ordered(stories, lambda story: f"{story.epic_id} - {story.epic}").items():
        lines.extend([f"### {epic_key}", ""])

        for feature, feature_stories in group_ordered(epic_stories, lambda story: story.feature).items():
            lines.extend([f"#### {feature}", ""])

            for story in feature_stories:
                lines.extend(
                    [
                        f"##### {story.story_id}",
                        "",
                        f"- Source requirement: {story.source_requirement}",
                        f"- Requirement type: {story.requirement_type}",
                        f"- User role: {story.user_role}",
                        f"- User story: {story.user_story}",
                        f"- Conditional applicability: {story.conditional_applicability}",
                        "- Acceptance criteria:",
                    ]
                )

                if story.acceptance_criteria:
                    for index, criterion in enumerate(story.acceptance_criteria, start=1):
                        lines.append(f"  {index}. {criterion}")
                else:
                    lines.append("  1. None recorded.")

                lines.extend(
                    [
                        f"- Open question / clarification: {story.open_question}",
                        f"- Source section: {story.source_section}",
                        f"- Source page: {story.source_page}",
                        f"- Source document: {story.source_document}",
                        "",
                    ]
                )

    lines.extend(
        [
            "## Source Trace Matrix",
            "",
            "| Technical Story | Source Requirement | Requirement Type | Epic | Feature | Source Section | Source Page |",
            "| --- | --- | --- | --- | --- | --- | --- |",
        ]
    )

    for story in stories:
        lines.append(
            f"| {story.story_id} | {story.source_requirement} | {story.requirement_type} | {story.epic_id} - {story.epic} | {story.feature} | {story.source_section} | {story.source_page} |"
        )

    lines.append("")
    OUTPUT.write_text("\n".join(lines), encoding="utf-8", newline="\n")


def main() -> None:
    stories = [to_story(row) for row in read_rows()]
    if len(stories) != EXPECTED_STORY_COUNT:
        raise RuntimeError(f"Expected {EXPECTED_STORY_COUNT} technical backlog rows, found {len(stories)}")

    missing_story_ids = [story.source_requirement for story in stories if not story.story_id]
    if missing_story_ids:
        raise RuntimeError(f"Missing technical story IDs for requirements: {', '.join(missing_story_ids)}")

    write_markdown(stories)
    print(f"Wrote {OUTPUT.relative_to(ROOT)} with {len(stories)} technical stories.")


if __name__ == "__main__":
    main()
