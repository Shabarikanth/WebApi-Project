namespace BugManagement.bug_dashboard.src.app.models
{
  // src/app/models/bug.ts
  export interface Bug {
    id?: number;           // optional because new bug doesn't have id yet
    title: string;
    description?: string;
    status: string;        // Open, Work In Progress, Closed, Hold, Rejected
    severity?: string;
    assignedTo?: string;
    createdAt?: string;
    updatedAt?: string;
  }

}
