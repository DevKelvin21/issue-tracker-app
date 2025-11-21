import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { issuesApi, type GetIssuesParams } from '../api/issues.api';
import type { CreateIssueDto, UpdateIssueDto } from '../types';

const ISSUES_QUERY_KEY = 'issues';

// Get all issues with optional status filter and pagination
export function useIssues(params: GetIssuesParams = {}) {
  const { status, pageNumber = 1, pageSize = 20 } = params;

  return useQuery({
    queryKey: [ISSUES_QUERY_KEY, status, pageNumber, pageSize],
    queryFn: () => issuesApi.getIssues({ status, pageNumber, pageSize }),
  });
}

// Get single issue
export function useIssue(id: number) {
  return useQuery({
    queryKey: [ISSUES_QUERY_KEY, id],
    queryFn: () => issuesApi.getIssue(id),
    enabled: !!id,
  });
}

// Create issue
export function useCreateIssue() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateIssueDto) => issuesApi.createIssue(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [ISSUES_QUERY_KEY] });
    },
  });
}

// Update issue
export function useUpdateIssue() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: UpdateIssueDto }) =>
      issuesApi.updateIssue(id, data),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: [ISSUES_QUERY_KEY] });
      queryClient.invalidateQueries({ queryKey: [ISSUES_QUERY_KEY, variables.id] });
    },
  });
}

// Resolve issue
export function useResolveIssue() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => issuesApi.resolveIssue(id),
    onSuccess: (_, id) => {
      queryClient.invalidateQueries({ queryKey: [ISSUES_QUERY_KEY] });
      queryClient.invalidateQueries({ queryKey: [ISSUES_QUERY_KEY, id] });
    },
  });
}

// Delete issue
export function useDeleteIssue() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => issuesApi.deleteIssue(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [ISSUES_QUERY_KEY] });
    },
  });
}
