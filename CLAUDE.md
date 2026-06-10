# Claude Code — Permanent Instructions

## 1. Plan System

### When to create a plan

Create a plan for any task that meets **one or more** of the following criteria:

- Touches more than 3 files
- Involves multiple steps or phases
- Carries a risk of breaking existing functionality
- Estimated duration exceeds 5 minutes

For simple, single-step tasks (quick edits, one-liner fixes, trivial lookups): **no plan needed**.

### Announce, then act

Before starting a qualifying task, output a single line:

```
📋 Creating plan: .plans/<filename>.md
```

Then immediately create the plan file and proceed. Do not ask for confirmation.

### On session resume

Do not attempt to auto-resume. Wait for the user to indicate which plan is concerned, then read that file in full before acting.

---

## 2. Plan File Naming Convention

```
.plans/<Title> - [YYYY-MM-DD HH-MM-SS] - [YYYY-MM-DD HH-MM-SS].md
                  ^ created (never updated)   ^ last modified (update on every write)
```

**Title format:** Title Case with spaces, concise, descriptive (e.g. `Refactor Auth Module`)

**Example:**

```
.plans/Refactor Auth Module - [2025-03-14 09-22-11] - [2025-03-14 11-47-03].md
```

> ⚠️ The creation datetime in the filename is **immutable**. Only the last-modified datetime changes.

---

## 3. Plan File Structure (Obsidian Markdown)

```markdown
---
title: "<Plan Title>"
description: "<One-sentence summary — update if scope changes>"
status: Planned | Active | Completed | Abandoned
tags: [tag1, tag2, ...]
created: YYYY-MM-DDTHH:MM:SS
modified: YYYY-MM-DDTHH:MM:SS
session: 1
previous_session: null | [[Previous Plan Title - ...]]
---

# <Plan Title>

## Description
> `YYYY-MM-DD HH:MM:SS`

<Full description of the request: context, goals, constraints, expected outcome.>

## Action Plan
> `YYYY-MM-DD HH:MM:SS`

- [ ] Step 1 — <description>
- [ ] Step 2 — <description>
- [ ] Step 3 — <description>
...

## Files Affected
> `YYYY-MM-DD HH:MM:SS` — updated on each file change

| File | Change | Note |
|------|--------|------|
| `path/to/file.ext` | created / modified / deleted | <git-commit-style note, one line, direct> |

## Corrections

<!-- Added only when an error, unexpected decision, or pivot occurs -->

### Correction 1
> `YYYY-MM-DD HH:MM:SS`

#### Description
<What went wrong or what unexpected situation arose.>

#### Solutions

##### Solution 1
<What was attempted or decided, and why.>

##### Solution 2 *(if applicable)*
<Alternative approach if the first failed.>

---
<!-- Repeat ### Correction N as needed -->
```

---

## 4. Tags — Shared Vocabulary

### Reading tags before writing

Before creating or updating any plan, **read the frontmatter (first 20 lines only) of every file in `.plans/`**. Extract the `tags:` field from each. Use existing tags whenever applicable. Introduce a new tag only when no existing one fits.

### Tag conventions

- Lowercase, hyphenated: `auth`, `api-refactor`, `db-migration`
- Functional: describe _what kind of work_ (e.g. `bugfix`, `feature`, `refactor`, `config`, `ci-cd`, `docs`, `testing`, `performance`)
- Domain: describe _which domain_ (e.g. `auth`, `frontend`, `backend`, `database`, `infra`)
- Combine both: `["bugfix", "auth", "backend"]`

### Updating stale tags on other plans

If, while reading frontmatter, you notice a tag on another plan is now inaccurate or redundant given new context, update **only the frontmatter** of that file (tags + description + modified). Never rewrite its content.

---

## 5. Obsidian Backlinks

Use `[[Plan Title - datetime - datetime]]` syntax (filename without `.md`) to cross-reference plans:

- **Multi-session continuity:** each new session plan links to its predecessor via `previous_session:` in frontmatter and a backlink in the Description section.
- **Related work:** if a plan touches the same domain as a recent plan, add a `> Related: [[...]]` callout at the end of the Description section.

---

## 6. Timestamping Rules

Add a `> YYYY-MM-DD HH:MM:SS` blockquote timestamp **only** on:

- Section headers at creation time (Description, Action Plan, Files Affected)
- Each new Correction block
- Each update to Files Affected
- Any significant decision or pivot inline in Action Plan steps (append, do not replace)

Format for inline step updates:

```markdown
- [x] Step 2 — Migrate users table `2025-03-14 10:15:44` ⚠️ schema conflict → see Correction 1
```

Do **not** timestamp every minor edit.

---

## 7. Action Plan — Execution Tracking

Mark steps as you go:

- `- [ ]` → not started
- `- [~]` → in progress
- `- [x]` → completed
- `- [!]` → blocked / failed → triggers a Correction section

---

## 8. Status Lifecycle

|Status|Meaning|
|---|---|
|`Planned`|File created, work not yet started|
|`Active`|Work in progress|
|`Completed`|All steps done, no blockers|
|`Abandoned`|Cancelled or superseded — add a note in Description|

Update `status:` and `modified:` in frontmatter on every status change.

---

## 9. Startup Checklist (every session)

1. Check if `.plans/` exists. If not, create it.
2. For each file in `.plans/`, read **only the first 20 lines** (frontmatter).
3. Build a mental map of: existing tags, active/planned tasks, related domains.
4. If the current task qualifies for a plan → announce + create plan file.
5. If resuming → wait for user to specify the plan, then read it in full.