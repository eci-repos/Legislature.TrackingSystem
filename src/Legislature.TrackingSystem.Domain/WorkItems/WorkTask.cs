using Legislature.TrackingSystem.Domain.Traceability;

namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A legislative work task aggregate root. Carries the attributes required to support
/// assignment, workflow, search, and reporting, plus an explicit unique identifier and
/// source trace reference.
/// </summary>
public sealed class WorkTask
{
    private readonly List<TaskAssignment> _assignments = new();
    private readonly List<string> _requiredReviewers = new();
    private readonly List<WorkflowReview> _reviews = new();
    private readonly List<ExecutiveReviewer> _executiveReviewers = new();
    private readonly List<ExecutiveReviewAdjustment> _adjustmentNotes = new();
    private readonly List<WorkflowStep> _steps = new();
    private readonly List<Attachment> _attachments = new();
    private readonly List<WorkTaskComment> _comments = new();
    private readonly List<WorkTaskAuditEntry> _auditEntries = new();
    private readonly List<WorkTaskVersion> _versions = new();

    private WorkTask()
    {
        Identifier = null!;
        Title = string.Empty;
        SourceTrace = null!;
    }

    private WorkTask(
        Guid id,
        WorkItemIdentifier identifier,
        WorkItemType type,
        string title,
        string? description,
        DateOnly? dueDate,
        TaskPriority priority,
        WorkTaskStatus status,
        string? owner,
        SourceTraceReference sourceTrace,
        DateTimeOffset createdAt,
        int year)
    {
        Id = id;
        Identifier = identifier;
        Type = type;
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        Status = status;
        Owner = owner;
        SourceTrace = sourceTrace;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        Year = year;
    }

    public Guid Id { get; private set; }

    public WorkItemIdentifier Identifier { get; private set; }

    public WorkItemType Type { get; private set; }

    public string Title { get; private set; }

    public string? Description { get; private set; }

    public DateOnly? DueDate { get; private set; }

    public TaskPriority Priority { get; private set; }

    public WorkTaskStatus Status { get; private set; }

    public string? Owner { get; private set; }

    public SourceTraceReference SourceTrace { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    /// <summary>
    /// The calendar year associated with this work product (US-10.2.1, B.EXP.01). Defaults to the
    /// creation year when not supplied (for example on legacy migration).
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// All assignments for this task, including superseded ones retained for history.
    /// </summary>
    public IReadOnlyList<TaskAssignment> Assignments => _assignments;

    /// <summary>
    /// The currently active (non-superseded) assignments for this task.
    /// </summary>
    public IEnumerable<TaskAssignment> ActiveAssignments => _assignments.Where(a => !a.IsSuperseded);

    /// <summary>
    /// Categorization flag: whether this work item is confidential (US-2.3.1).
    /// </summary>
    public bool IsConfidential { get; private set; }

    /// <summary>
    /// Categorization flag: whether this work item is under executive review (US-2.3.1).
    /// </summary>
    public bool IsExecutiveReview { get; private set; }

    /// <summary>
    /// The review and approval workflow state of this work item (US-3.1.1, B.COM.15).
    /// Defaults to <see cref="WorkflowStatus.Draft"/> when the task is created.
    /// </summary>
    public WorkflowStatus WorkflowStatus { get; private set; } = WorkflowStatus.Draft;

    /// <summary>
    /// The reviewer user keys required to approve the current review cycle (US-3.1.2).
    /// </summary>
    public IReadOnlyList<string> RequiredReviewerKeys => _requiredReviewers;

    private string[] RequiredReviewerKeyValues
    {
        get => _requiredReviewers.ToArray();
        set
        {
            _requiredReviewers.Clear();
            if (value is not null)
            {
                _requiredReviewers.AddRange(value);
            }
        }
    }

    /// <summary>
    /// The retained review history for this work item.
    /// </summary>
    public IReadOnlyList<WorkflowReview> Reviews => _reviews;

    /// <summary>
    /// The lifecycle status of the RFA Executive Review path (US-3.2.1, B.RFA.01).
    /// </summary>
    public ExecutiveReviewStatus ExecutiveReviewStatus { get; private set; } = ExecutiveReviewStatus.NotStarted;

    /// <summary>
    /// The ordered executive reviewers in the Executive Review path (US-3.2.2, B.RFA.02).
    /// </summary>
    public IReadOnlyList<ExecutiveReviewer> ExecutiveReviewers => _executiveReviewers;

    /// <summary>
    /// Retained adjustment/correction notes recorded by executive reviewers (US-3.2.2).
    /// </summary>
    public IReadOnlyList<ExecutiveReviewAdjustment> AdjustmentNotes => _adjustmentNotes;

    /// <summary>
    /// The workflow steps or subtasks within this work product, each with its own due date
    /// (US-3.2.3, B.RFA.04).
    /// </summary>
    public IReadOnlyList<WorkflowStep> Steps => _steps;

    /// <summary>
    /// The authored content of this work product (rich text for most types, limited for fiscal
    /// notes) (US-4.1.x). Saving content does not require completion or approval (US-4.2.2).
    /// </summary>
    public string? Content { get; private set; }

    /// <summary>
    /// The timestamp of the most recent content save (US-4.2.2, B.COM.22).
    /// </summary>
    public DateTimeOffset? LastSavedAt { get; private set; }

    /// <summary>
    /// The documents attached to this work task (US-4.2.1, B.COM.21).
    /// </summary>
    public IReadOnlyList<Attachment> Attachments => _attachments;

    /// <summary>
    /// The customer due date for this work product (US-1.3.4, B.COM.14). It remains associated
    /// with the product throughout its workflow.
    /// </summary>
    public DateOnly? CustomerDueDate { get; private set; }

    /// <summary>
    /// The collaboration comments on this work task (US-1.1.1, B.COM.01).
    /// </summary>
    public IReadOnlyList<WorkTaskComment> Comments => _comments;

    /// <summary>
    /// The audit trail of task maintenance actions (US-1.4.1, B.COM.18; US-1.4.2, B.COM.23).
    /// </summary>
    public IReadOnlyList<WorkTaskAuditEntry> AuditEntries => _auditEntries;

    /// <summary>
    /// The retained versions of this work product (US-5.2.2, B.COM.35). An update does not
    /// overwrite the only copy of the previous version.
    /// </summary>
    public IReadOnlyList<WorkTaskVersion> Versions => _versions;

    public static WorkTask Create(
        WorkItemIdentifier identifier,
        WorkItemType type,
        string title,
        string? description,
        DateOnly? dueDate,
        TaskPriority priority,
        WorkTaskStatus status,
        string? owner,
        SourceTraceReference sourceTrace,
        DateTimeOffset createdAt,
        int? year = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A work task title is required.", nameof(title));
        }

        return new WorkTask(
            Guid.NewGuid(),
            identifier,
            type,
            title.Trim(),
            description,
            dueDate,
            priority,
            status,
            owner,
            sourceTrace,
            createdAt,
            year ?? createdAt.Year);
    }

    public void OverrideIdentifier(WorkItemIdentifier newIdentifier, DateTimeOffset updatedAt)
    {
        Identifier = newIdentifier;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Assigns a DOR user to this task for the given role. A user who previously held an
    /// assignment on this task is flagged as rework on re-engagement.
    /// </summary>
    public TaskAssignment AssignUser(
        string assigneeKey,
        AssignmentRole role,
        DateOnly? dueDate,
        string? assignedByKey,
        DateTimeOffset assignedAt)
    {
        bool previouslyHeld = _assignments.Any(a =>
            string.Equals(a.AssigneeKey, assigneeKey, StringComparison.OrdinalIgnoreCase));

        TaskAssignment assignment = TaskAssignment.Create(
            assigneeKey,
            role,
            dueDate,
            assignedByKey,
            assignedAt,
            isRework: previouslyHeld);

        _assignments.Add(assignment);
        UpdatedAt = assignedAt;
        return assignment;
    }

    /// <summary>
    /// Reassigns the task from one DOR user to another. The prior assignee's active assignments
    /// are superseded (retained for history) and the new assignee receives a fresh assignment;
    /// the work product and supporting information on the task are never removed.
    /// </summary>
    public TaskAssignment ReassignUser(
        string priorAssigneeKey,
        string newAssigneeKey,
        AssignmentRole role,
        DateOnly? dueDate,
        string? assignedByKey,
        DateTimeOffset assignedAt)
    {
        foreach (TaskAssignment assignment in _assignments.Where(a =>
            !a.IsSuperseded &&
            string.Equals(a.AssigneeKey, priorAssigneeKey, StringComparison.OrdinalIgnoreCase)))
        {
            assignment.Supersede();
        }

        return AssignUser(newAssigneeKey, role, dueDate, assignedByKey, assignedAt);
    }

    /// <summary>
    /// Sets the categorization flags (confidential, executive review) used for sorting,
    /// filtering, and grouping (US-2.3.1).
    /// </summary>
    public void SetCategorization(bool isConfidential, bool isExecutiveReview, DateTimeOffset updatedAt)
    {
        IsConfidential = isConfidential;
        IsExecutiveReview = isExecutiveReview;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Submits this work item for review (US-3.1.1, B.COM.15). The task moves to
    /// <see cref="WorkflowStatus.PendingReview"/> with the given required reviewers. Work and
    /// approval responsibilities must be separated: at least one reviewer must not be a user
    /// performing the work.
    /// </summary>
    public void SubmitForReview(
        IReadOnlyList<string> reviewerKeys,
        DateTimeOffset submittedAt)
    {
        if (WorkflowStatus is not (WorkflowStatus.Draft or WorkflowStatus.Rejected))
        {
            throw new InvalidOperationException(
                $"Work item '{Identifier.Value}' cannot be submitted for review from workflow state '{WorkflowStatus}'.");
        }

        if (reviewerKeys is null || reviewerKeys.Count == 0)
        {
            throw new ArgumentException("At least one reviewer is required to submit work for review.", nameof(reviewerKeys));
        }

        EnsureSeparationOfDuties(reviewerKeys);

        _requiredReviewers.Clear();
        foreach (string key in reviewerKeys.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            _requiredReviewers.Add(key);
        }

        WorkflowStatus = WorkflowStatus.PendingReview;
        UpdatedAt = submittedAt;
    }

    /// <summary>
    /// Records a single reviewer's decision (US-3.1.2, B.COM.19). When every required reviewer
    /// has approved, the item is <see cref="WorkflowStatus.Approved"/>; any rejection moves it to
    /// <see cref="WorkflowStatus.Rejected"/>. Review history is retained.
    /// </summary>
    public void RecordReview(string reviewerKey, WorkflowDecision decision, string? comment, DateTimeOffset reviewedAt)
    {
        if (WorkflowStatus is not (WorkflowStatus.PendingReview or WorkflowStatus.UnderReview))
        {
            throw new InvalidOperationException(
                $"Work item '{Identifier.Value}' is not awaiting review (workflow state '{WorkflowStatus}').");
        }

        if (!_requiredReviewers.Contains(reviewerKey, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"User '{reviewerKey}' is not an assigned reviewer for '{Identifier.Value}'.");
        }

        if (_reviews.Any(r => string.Equals(r.ReviewerKey, reviewerKey, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                $"User '{reviewerKey}' has already recorded a review for '{Identifier.Value}'.");
        }

        _reviews.Add(new WorkflowReview(Guid.NewGuid(), reviewerKey, decision, comment, reviewedAt));

        if (decision == WorkflowDecision.Rejected)
        {
            WorkflowStatus = WorkflowStatus.Rejected;
        }
        else
        {
            IEnumerable<string> approved = _reviews
                .Where(r => r.Decision == WorkflowDecision.Approved)
                .Select(r => r.ReviewerKey);

            bool allRequiredApproved = _requiredReviewers.All(k =>
                approved.Contains(k, StringComparer.OrdinalIgnoreCase));

            WorkflowStatus = allRequiredApproved ? WorkflowStatus.Approved : WorkflowStatus.UnderReview;
        }

        UpdatedAt = reviewedAt;
    }

    /// <summary>
    /// Finalizes an approved work item for packaging and submission (US-3.1.3, B.COM.20). Only
    /// items in <see cref="WorkflowStatus.Approved"/> can be finalized.
    /// </summary>
    public void Finalize(DateTimeOffset finalizedAt)
    {
        if (WorkflowStatus != WorkflowStatus.Approved)
        {
            throw new InvalidOperationException(
                $"Only approved work items can be finalized; '{Identifier.Value}' is '{WorkflowStatus}'.");
        }

        WorkflowStatus = WorkflowStatus.Finalized;
        UpdatedAt = finalizedAt;
    }

    /// <summary>
    /// Sets the priority of this work product so limited resources can be focused on the most
    /// urgent legislative work (US-3.2.4, B.RFA.05).
    /// </summary>
    public void SetPriority(TaskPriority priority, DateTimeOffset updatedAt)
    {
        Priority = priority;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Starts the RFA Executive Review path for this fiscal product (US-3.2.1, B.RFA.01). The
    /// given reviewers are designated in order; the review proceeds sequentially until every
    /// reviewer has completed. The caller enforces that reviewers are designated users.
    /// </summary>
    public void StartExecutiveReview(IReadOnlyList<string> reviewerKeys, DateTimeOffset startedAt)
    {
        if (ExecutiveReviewStatus != ExecutiveReviewStatus.NotStarted)
        {
            throw new InvalidOperationException(
                $"Executive review for '{Identifier.Value}' has already started (state '{ExecutiveReviewStatus}').");
        }

        if (reviewerKeys is null || reviewerKeys.Count == 0)
        {
            throw new ArgumentException("At least one executive reviewer is required.", nameof(reviewerKeys));
        }

        _executiveReviewers.Clear();
        int order = 1;
        foreach (string key in reviewerKeys.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            _executiveReviewers.Add(new ExecutiveReviewer(
                Guid.NewGuid(),
                key,
                order++,
                ExecutiveReviewerStatus.Pending,
                null,
                null));
        }

        ExecutiveReviewStatus = ExecutiveReviewStatus.InProgress;
        UpdatedAt = startedAt;
    }

    /// <summary>
    /// Marks the current executive reviewer's step as in progress (US-3.2.2). Only the current
    /// reviewer in the sequential path can begin their step.
    /// </summary>
    public void BeginExecutiveReviewStep(string reviewerKey, DateTimeOffset at)
    {
        EnsureExecutiveReviewInProgress();
        ExecutiveReviewer reviewer = GetCurrentExecutiveReviewer();
        EnsureIsCurrentReviewer(reviewer, reviewerKey);
        ReplaceExecutiveReviewer(reviewer with { Status = ExecutiveReviewerStatus.InProgress });
        UpdatedAt = at;
    }

    /// <summary>
    /// Records an adjustment or correction by the current executive reviewer (US-3.2.2).
    /// </summary>
    public void AdjustExecutiveReview(string reviewerKey, string note, DateTimeOffset at)
    {
        EnsureExecutiveReviewInProgress();
        ExecutiveReviewer reviewer = GetCurrentExecutiveReviewer();
        EnsureIsCurrentReviewer(reviewer, reviewerKey);

        if (string.IsNullOrWhiteSpace(note))
        {
            throw new ArgumentException("An adjustment note is required.", nameof(note));
        }

        _adjustmentNotes.Add(new ExecutiveReviewAdjustment(Guid.NewGuid(), reviewerKey, note.Trim(), at));
        UpdatedAt = at;
    }

    /// <summary>
    /// Completes the current executive reviewer's step (US-3.2.2). When the last reviewer
    /// completes, the Executive Review path is <see cref="ExecutiveReviewStatus.Completed"/>;
    /// otherwise the next reviewer becomes current (the automatic handoff).
    /// </summary>
    public void CompleteExecutiveReviewStep(string reviewerKey, string? comment, DateTimeOffset at)
    {
        EnsureExecutiveReviewInProgress();
        ExecutiveReviewer reviewer = GetCurrentExecutiveReviewer();
        EnsureIsCurrentReviewer(reviewer, reviewerKey);

        ReplaceExecutiveReviewer(reviewer with
        {
            Status = ExecutiveReviewerStatus.Completed,
            Comment = comment,
            CompletedAt = at,
        });

        if (_executiveReviewers.All(r => r.Status == ExecutiveReviewerStatus.Completed))
        {
            ExecutiveReviewStatus = ExecutiveReviewStatus.Completed;
        }

        UpdatedAt = at;
    }

    /// <summary>
    /// Adds a workflow step or subtask to this work product with its own due date
    /// (US-3.2.3, B.RFA.04).
    /// </summary>
    public WorkflowStep AddStep(string name, DateOnly? dueDate, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A workflow step requires a name.", nameof(name));
        }

        var step = new WorkflowStep(Guid.NewGuid(), name.Trim(), dueDate, WorkflowStepStatus.Pending);
        _steps.Add(step);
        UpdatedAt = at;
        return step;
    }

    /// <summary>
    /// Sets the due date of an existing workflow step (US-3.2.3).
    /// </summary>
    public void SetStepDueDate(Guid stepId, DateOnly? dueDate, DateTimeOffset at)
    {
        int index = _steps.FindIndex(s => s.Id == stepId);
        if (index < 0)
        {
            throw new InvalidOperationException($"Workflow step '{stepId}' was not found on '{Identifier.Value}'.");
        }

        _steps[index] = _steps[index] with { DueDate = dueDate };
        UpdatedAt = at;
    }

    /// <summary>
    /// Marks an existing workflow step as completed (US-3.2.3).
    /// </summary>
    public void CompleteStep(Guid stepId, DateTimeOffset at)
    {
        int index = _steps.FindIndex(s => s.Id == stepId);
        if (index < 0)
        {
            throw new InvalidOperationException($"Workflow step '{stepId}' was not found on '{Identifier.Value}'.");
        }

        _steps[index] = _steps[index] with { Status = WorkflowStepStatus.Completed };
        UpdatedAt = at;
    }

    /// <summary>
    /// Saves the authored content of this work product (US-4.1.x). Saving is allowed at any time
    /// and does not require completion or approval, so incomplete work can be saved and resumed
    /// (US-4.2.2, B.COM.22).
    /// </summary>
    public void SetContent(string? content, DateTimeOffset at)
    {
        _versions.Add(new WorkTaskVersion(Guid.NewGuid(), Id, _versions.Count + 1, content, at, null));
        Content = content;
        LastSavedAt = at;
        UpdatedAt = at;
    }

    /// <summary>
    /// Attaches a document to this work task (US-4.2.1, B.COM.21). Multiple attachments of
    /// supported formats remain associated with the task.
    /// </summary>
    public Attachment AddAttachment(string fileName, string contentType, long sizeBytes, string? addedByKey, DateTimeOffset at)
    {
        Attachment attachment = Attachment.Create(fileName, contentType, sizeBytes, addedByKey, at);
        _attachments.Add(attachment);
        UpdatedAt = at;
        return attachment;
    }

    /// <summary>
    /// Removes an attachment from this work task (US-4.2.1).
    /// </summary>
    public void RemoveAttachment(Guid attachmentId, DateTimeOffset at)
    {
        Attachment? attachment = _attachments.FirstOrDefault(a => a.Id == attachmentId);
        if (attachment is null)
        {
            throw new InvalidOperationException($"Attachment '{attachmentId}' was not found on '{Identifier.Value}'.");
        }

        _attachments.Remove(attachment);
        UpdatedAt = at;
    }

    /// <summary>
    /// Transfers applicable content from a source work product into this product without
    /// copy-and-paste (US-4.4.1, B.COM.28; US-4.4.2, B.COM.29). The destination product remains
    /// independently identifiable because it is a distinct work task with its own identifier.
    /// </summary>
    public void ReuseContentFrom(WorkTask source, DateTimeOffset at)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (source.Id == Id)
        {
            throw new InvalidOperationException("A work product cannot reuse content from itself.");
        }

        Content = source.Content;
        LastSavedAt = at;
        UpdatedAt = at;
    }

    /// <summary>
    /// Sets the customer due date for this work product (US-1.3.4, B.COM.14).
    /// </summary>
    public void SetCustomerDueDate(DateOnly? dueDate, DateTimeOffset at)
    {
        CustomerDueDate = dueDate;
        UpdatedAt = at;
    }

    /// <summary>
    /// Adds a collaboration comment to this work task (US-1.1.1, B.COM.01).
    /// </summary>
    public WorkTaskComment AddComment(string authorKey, string body, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(authorKey))
        {
            throw new ArgumentException("A comment requires an author.", nameof(authorKey));
        }

        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ArgumentException("A comment requires a body.", nameof(body));
        }

        WorkTaskComment comment = new(Guid.NewGuid(), authorKey.Trim(), body.Trim(), at);
        _comments.Add(comment);
        UpdatedAt = at;
        return comment;
    }

    /// <summary>
    /// Updates applicable task information (US-1.4.1, B.COM.18). The prior state is recorded in
    /// the audit trail so it is not silently lost (US-1.4.2, B.COM.23).
    /// </summary>
    public void UpdateTask(string? title, string? description, DateOnly? dueDate, TaskPriority? priority, string? byKey, DateTimeOffset at)
    {
        string prior = $"Title='{Title}'; Status='{Status}'; Priority='{Priority}'; DueDate='{DueDate?.ToString("yyyy-MM-dd") ?? "none"}'";

        if (title is not null)
        {
            Title = title.Trim();
        }

        if (description is not null)
        {
            Description = description;
        }

        if (dueDate is not null)
        {
            DueDate = dueDate;
        }

        if (priority is not null)
        {
            Priority = priority.Value;
        }

        UpdatedAt = at;
        _auditEntries.Add(new WorkTaskAuditEntry(Guid.NewGuid(), "Update", $"Prior: {prior}", at, byKey));
    }

    /// <summary>
    /// Cancels this work task (US-1.4.1, B.COM.18). The prior state is recorded in the audit trail
    /// so it is not silently lost (US-1.4.2, B.COM.23).
    /// </summary>
    public void CancelTask(string? byKey, DateTimeOffset at)
    {
        if (Status == WorkTaskStatus.Canceled)
        {
            throw new InvalidOperationException($"Work item '{Identifier.Value}' is already canceled.");
        }

        string prior = $"Status='{Status}'";
        Status = WorkTaskStatus.Canceled;
        UpdatedAt = at;
        _auditEntries.Add(new WorkTaskAuditEntry(Guid.NewGuid(), "Cancel", $"Prior: {prior}", at, byKey));
    }

    private void EnsureExecutiveReviewInProgress()
    {
        if (ExecutiveReviewStatus != ExecutiveReviewStatus.InProgress)
        {
            throw new InvalidOperationException(
                $"Executive review for '{Identifier.Value}' is not in progress (state '{ExecutiveReviewStatus}').");
        }
    }

    private ExecutiveReviewer GetCurrentExecutiveReviewer()
    {
        return _executiveReviewers.First(r => r.Status != ExecutiveReviewerStatus.Completed);
    }

    private void EnsureIsCurrentReviewer(ExecutiveReviewer reviewer, string reviewerKey)
    {
        if (!string.Equals(reviewer.ReviewerKey, reviewerKey, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"User '{reviewerKey}' is not the current executive reviewer for '{Identifier.Value}'.");
        }
    }

    private void ReplaceExecutiveReviewer(ExecutiveReviewer updated)
    {
        int index = _executiveReviewers.FindIndex(r => r.Id == updated.Id);
        _executiveReviewers[index] = updated;
    }

    private void EnsureSeparationOfDuties(IReadOnlyList<string> reviewerKeys)
    {
        // Preparers are active assignees performing the work (Owner/Analyst).
        var preparers = new HashSet<string>(
            ActiveAssignments
                .Where(a => a.Role is AssignmentRole.Owner or AssignmentRole.Analyst)
                .Select(a => a.AssigneeKey),
            StringComparer.OrdinalIgnoreCase);

        bool hasIndependentReviewer = reviewerKeys.Any(key =>
            !preparers.Contains(key, StringComparer.OrdinalIgnoreCase));

        if (!hasIndependentReviewer)
        {
            throw new InvalidOperationException(
                "Work and approval responsibilities must be separated: at least one reviewer must not be a user performing the work.");
        }
    }
}
