/**
 * Issue status enumeration
 */
export enum IssueStatus {
  Open = 1,
  InProgress = 2,
  Resolved = 3,
}

/**
 * Issue domain model
 * Represents an issue in the system
 */
export interface Issue {
  id: number;
  title: string;
  description: string;
  status: IssueStatus;
  createdAt: string;
  resolvedAt: string | null;
}
