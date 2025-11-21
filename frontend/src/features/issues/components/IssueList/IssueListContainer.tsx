import { useIssueListState } from '../../hooks/use-issue-list-state';
import { IssueListView } from './IssueListView';
import type { Issue } from '../../../../types/domain';

/**
 * Container component for IssueList.
 * Handles all business logic and state management,
 * passing only data and callbacks to the presentation component.
 */
export function IssueListContainer() {
  const state = useIssueListState();

  const handleCreateClick = () => {
    state.setIsCreating(true);
  };

  const handleEditIssue = (issue: Issue) => {
    state.setEditingIssue(issue);
  };

  const handleFormSuccess = () => {
    state.setIsCreating(false);
    state.setEditingIssue(null);
  };

  const handleFormCancel = () => {
    state.setIsCreating(false);
    state.setEditingIssue(null);
  };

  return (
    <IssueListView
      issues={state.issues}
      isLoading={state.isLoading}
      error={state.error}
      statusFilter={state.statusFilter}
      pageSize={state.pageSize}
      onStatusChange={state.setStatusFilter}
      onPageSizeChange={state.handlePageSizeChange}
      paginationData={state.paginationData}
      onPageChange={state.handlePageChange}
      isCreating={state.isCreating}
      editingIssue={state.editingIssue}
      onCreateClick={handleCreateClick}
      onEditIssue={handleEditIssue}
      onFormSuccess={handleFormSuccess}
      onFormCancel={handleFormCancel}
    />
  );
}
