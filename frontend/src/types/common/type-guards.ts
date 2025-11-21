import { IssueStatus } from '../domain';
import type { Issue } from '../domain';
import type { ProblemDetails } from './api';

/**
 * Type guard to check if a value is a valid IssueStatus
 */
export function isIssueStatus(value: unknown): value is IssueStatus {
  return (
    typeof value === 'number' &&
    (value === IssueStatus.Open || value === IssueStatus.InProgress || value === IssueStatus.Resolved)
  );
}

/**
 * Type guard to check if a value is a valid Issue object
 */
export function isIssue(value: unknown): value is Issue {
  if (typeof value !== 'object' || value === null) {
    return false;
  }

  const obj = value as Record<string, unknown>;

  return (
    typeof obj.id === 'number' &&
    typeof obj.title === 'string' &&
    typeof obj.description === 'string' &&
    isIssueStatus(obj.status) &&
    typeof obj.createdAt === 'string' &&
    (obj.resolvedAt === null || typeof obj.resolvedAt === 'string')
  );
}

/**
 * Type guard to check if a value is a ProblemDetails error response
 */
export function isProblemDetails(value: unknown): value is ProblemDetails {
  if (typeof value !== 'object' || value === null) {
    return false;
  }

  const obj = value as Record<string, unknown>;

  return typeof obj.title === 'string' && typeof obj.status === 'number';
}
