import { useResolveIssue, useDeleteIssue } from '../../hooks/use-issues';
import { useConfirmDialog } from '../../../../hooks';
import { IssueCardView } from './IssueCardView';
import type { Issue } from '../../../../types/domain';

export interface IssueCardContainerProps {
  issue: Issue;
  onEdit: (issue: Issue) => void;
}

/**
 * Container component for IssueCard.
 * Handles all business logic including confirmations, mutations, and error handling.
 */
export function IssueCardContainer({ issue, onEdit }: IssueCardContainerProps) {
  const resolveIssueMutation = useResolveIssue();
  const deleteIssueMutation = useDeleteIssue();
  const { confirm } = useConfirmDialog();

  const handleResolve = async () => {
    const confirmed = await confirm('Mark this issue as resolved?');
    if (confirmed) {
      try {
        await resolveIssueMutation.mutateAsync(issue.id);
      } catch (error) {
        console.error('Failed to resolve issue:', error);
      }
    }
  };

  const handleDelete = async () => {
    const confirmed = await confirm('Are you sure you want to delete this issue?');
    if (confirmed) {
      try {
        await deleteIssueMutation.mutateAsync(issue.id);
      } catch (error) {
        console.error('Failed to delete issue:', error);
      }
    }
  };

  const handleEdit = () => {
    onEdit(issue);
  };

  return (
    <IssueCardView
      issue={issue}
      isResolving={resolveIssueMutation.isPending}
      isDeleting={deleteIssueMutation.isPending}
      onEdit={handleEdit}
      onResolve={handleResolve}
      onDelete={handleDelete}
    />
  );
}
