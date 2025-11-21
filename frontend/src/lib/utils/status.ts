import { IssueStatus } from '../../types/domain';

/**
 * Get the human-readable label for an issue status
 */
export function getStatusLabel(status: IssueStatus): string {
  switch (status) {
    case IssueStatus.Open:
      return 'Open';
    case IssueStatus.InProgress:
      return 'In Progress';
    case IssueStatus.Resolved:
      return 'Resolved';
    default:
      return 'Unknown';
  }
}

/**
 * Get the Tailwind CSS classes for an issue status badge
 */
export function getStatusColor(status: IssueStatus): string {
  switch (status) {
    case IssueStatus.Open:
      return 'bg-blue-100 text-blue-800 border-blue-200';
    case IssueStatus.InProgress:
      return 'bg-yellow-100 text-yellow-800 border-yellow-200';
    case IssueStatus.Resolved:
      return 'bg-green-100 text-green-800 border-green-200';
    default:
      return 'bg-gray-100 text-gray-800 border-gray-200';
  }
}
