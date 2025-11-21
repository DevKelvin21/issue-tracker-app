import { Card, Button, LoadingSpinner, Pagination } from '../../../../components/ui';
import { IssueCard } from '../IssueCard';
import { IssueForm } from '../IssueForm';
import { IssueFilters } from './IssueFilters';
import type { Issue, IssueStatus } from '../../../../types/domain';
import { getStatusLabel } from '../../../../lib/utils';

export interface IssueListViewProps {
  // Data
  issues: Issue[];
  isLoading: boolean;
  error: Error | null;

  // Filters
  statusFilter: IssueStatus | undefined;
  pageSize: number;
  onStatusChange: (status: IssueStatus | undefined) => void;
  onPageSizeChange: (size: number) => void;

  // Pagination
  paginationData: {
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasPrevious: boolean;
    hasNext: boolean;
  } | null;
  onPageChange: (page: number) => void;

  // Modal state
  isCreating: boolean;
  editingIssue: Issue | null;
  onCreateClick: () => void;
  onEditIssue: (issue: Issue) => void;
  onFormSuccess: () => void;
  onFormCancel: () => void;
}

export function IssueListView({
  issues,
  isLoading,
  error,
  statusFilter,
  pageSize,
  onStatusChange,
  onPageSizeChange,
  paginationData,
  onPageChange,
  isCreating,
  editingIssue,
  onCreateClick,
  onEditIssue,
  onFormSuccess,
  onFormCancel,
}: IssueListViewProps) {
  if (error) {
    return (
      <Card>
        <div className="bg-red-50 border border-red-200 rounded-lg p-4 text-red-800">
          <h3 className="font-semibold mb-1">Error loading issues</h3>
          <p className="text-sm">{error.message}</p>
        </div>
      </Card>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header and Filters */}
      <Card>
        <div className="flex items-center justify-between mb-4">
          <h1 className="text-3xl font-bold text-gray-900">Issue Tracker</h1>
          <Button onClick={onCreateClick} disabled={isCreating || !!editingIssue}>
            Create Issue
          </Button>
        </div>

        <IssueFilters
          statusFilter={statusFilter}
          pageSize={pageSize}
          onStatusChange={onStatusChange}
          onPageSizeChange={onPageSizeChange}
        />
      </Card>

      {/* Form Section */}
      {(isCreating || editingIssue) && (
        <IssueForm issue={editingIssue || undefined} onSuccess={onFormSuccess} onCancel={onFormCancel} />
      )}

      {/* Loading State */}
      {isLoading && <LoadingSpinner message="Loading issues..." />}

      {/* Issues Grid */}
      {!isLoading && issues.length > 0 && (
        <>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
            {issues.map((issue) => (
              <IssueCard key={issue.id} issue={issue} onEdit={onEditIssue} />
            ))}
          </div>

          {/* Pagination */}
          {paginationData && (
            <Card>
              <Pagination
                currentPage={paginationData.pageNumber}
                totalPages={paginationData.totalPages}
                pageSize={paginationData.pageSize}
                totalCount={paginationData.totalCount}
                hasPrevious={paginationData.hasPrevious}
                hasNext={paginationData.hasNext}
                onPageChange={onPageChange}
              />
            </Card>
          )}
        </>
      )}

      {/* Empty State */}
      {!isLoading && issues.length === 0 && (
        <Card>
          <div className="text-center py-12">
            <svg
              className="mx-auto h-12 w-12 text-gray-400"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
              aria-hidden="true"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
              />
            </svg>
            <h3 className="mt-2 text-sm font-medium text-gray-900">No issues found</h3>
            <p className="mt-1 text-sm text-gray-500">
              {statusFilter ? `No issues with status "${getStatusLabel(statusFilter)}"` : 'Get started by creating a new issue.'}
            </p>
          </div>
        </Card>
      )}
    </div>
  );
}
