import { useState } from 'react';
import { usePagination } from '../../../hooks';
import { useIssues } from './use-issues';
import type { Issue, IssueStatus } from '../../../types/domain';

export interface UseIssueListStateReturn {
  // Filters
  statusFilter: IssueStatus | undefined;
  setStatusFilter: (status: IssueStatus | undefined) => void;

  // Pagination
  pageNumber: number;
  pageSize: number;
  handlePageChange: (page: number) => void;
  handlePageSizeChange: (size: number) => void;

  // Modal state
  isCreating: boolean;
  editingIssue: Issue | null;
  setIsCreating: (value: boolean) => void;
  setEditingIssue: (issue: Issue | null) => void;

  // Data
  issues: Issue[];
  isLoading: boolean;
  error: Error | null;
  paginationData: {
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasPrevious: boolean;
    hasNext: boolean;
  } | null;
}

export function useIssueListState(): UseIssueListStateReturn {
  const [statusFilter, setStatusFilter] = useState<IssueStatus | undefined>(undefined);
  const [isCreating, setIsCreating] = useState(false);
  const [editingIssue, setEditingIssue] = useState<Issue | null>(null);

  const pagination = usePagination({
    initialPage: 1,
    initialPageSize: 20,
  });

  // Fetch issues with current filters and pagination
  const { data: paginatedData, isLoading, error } = useIssues({
    status: statusFilter,
    pageNumber: pagination.pageNumber,
    pageSize: pagination.pageSize,
  });

  const issues = paginatedData?.items ?? [];

  // When status filter changes, reset to first page
  const handleStatusFilterChange = (status: IssueStatus | undefined) => {
    setStatusFilter(status);
    pagination.setPageNumber(1);
  };

  const handlePageSizeChange = (size: number) => {
    pagination.handlePageSizeChange(size);
  };

  return {
    statusFilter,
    setStatusFilter: handleStatusFilterChange,
    pageNumber: pagination.pageNumber,
    pageSize: pagination.pageSize,
    handlePageChange: pagination.handlePageChange,
    handlePageSizeChange,
    isCreating,
    editingIssue,
    setIsCreating,
    setEditingIssue,
    issues,
    isLoading,
    error: error as Error | null,
    paginationData: paginatedData
      ? {
          pageNumber: paginatedData.pageNumber,
          pageSize: paginatedData.pageSize,
          totalCount: paginatedData.totalCount,
          totalPages: paginatedData.totalPages,
          hasPrevious: paginatedData.hasPrevious,
          hasNext: paginatedData.hasNext,
        }
      : null,
  };
}
