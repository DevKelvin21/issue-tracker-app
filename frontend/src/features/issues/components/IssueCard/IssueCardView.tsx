import { Card, Button } from '../../../../components/ui';
import type { Issue } from '../../../../types/domain';
import { getStatusLabel, getStatusColor, formatDate } from '../../../../lib/utils';
import { IssueStatus } from '../../../../types/domain';

export interface IssueCardViewProps {
  issue: Issue;
  isResolving: boolean;
  isDeleting: boolean;
  onEdit: () => void;
  onResolve: () => void;
  onDelete: () => void;
}

export function IssueCardView({ issue, isResolving, isDeleting, onEdit, onResolve, onDelete }: IssueCardViewProps) {
  return (
    <Card padding="md" className="hover:shadow-lg transition-shadow">
      <div className="flex items-start justify-between mb-3">
        <h3 className="text-xl font-semibold text-gray-900 flex-1">{issue.title}</h3>
        <span className={`px-3 py-1 rounded-full text-xs font-medium border ${getStatusColor(issue.status)}`}>
          {getStatusLabel(issue.status)}
        </span>
      </div>

      <p className="text-gray-600 mb-4 line-clamp-3">{issue.description}</p>

      <div className="flex items-center justify-between text-sm text-gray-500 mb-4">
        <span>Created: {formatDate(issue.createdAt)}</span>
        {issue.resolvedAt && <span>Resolved: {formatDate(issue.resolvedAt)}</span>}
      </div>

      <div className="flex gap-2">
        <Button onClick={onEdit} size="sm">
          Edit
        </Button>

        {issue.status !== IssueStatus.Resolved && (
          <Button variant="success" size="sm" onClick={onResolve} isLoading={isResolving} disabled={isResolving}>
            {isResolving ? 'Resolving...' : 'Resolve'}
          </Button>
        )}

        <Button variant="danger" size="sm" onClick={onDelete} isLoading={isDeleting} disabled={isDeleting} className="ml-auto">
          {isDeleting ? 'Deleting...' : 'Delete'}
        </Button>
      </div>
    </Card>
  );
}
