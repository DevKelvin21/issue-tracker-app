import apiClient from '../../../lib/api-client';
import type { PaginatedResponse } from '../../../types/common';
import type { Issue, IssueStatus } from '../../../types/domain';
import type { CreateIssueDto, UpdateIssueDto } from '../types';

export interface GetIssuesParams {
  status?: IssueStatus;
  pageNumber?: number;
  pageSize?: number;
}

export const issuesApi = {
  // Get all issues with optional status filter and pagination
  getIssues: async (params: GetIssuesParams = {}): Promise<PaginatedResponse<Issue>> => {
    const { status, pageNumber = 1, pageSize = 20 } = params;
    const queryParams: Record<string, string | number> = {
      pageNumber,
      pageSize,
    };

    if (status !== undefined) {
      queryParams.status = status;
    }

    const response = await apiClient.get<PaginatedResponse<Issue>>('/issues', { params: queryParams });
    return response.data;
  },

  // Get single issue by ID
  getIssue: async (id: number): Promise<Issue> => {
    const response = await apiClient.get<Issue>(`/issues/${id}`);
    return response.data;
  },

  // Create new issue
  createIssue: async (data: CreateIssueDto): Promise<Issue> => {
    const response = await apiClient.post<Issue>('/issues', data);
    return response.data;
  },

  // Update existing issue
  updateIssue: async (id: number, data: UpdateIssueDto): Promise<Issue> => {
    const response = await apiClient.put<Issue>(`/issues/${id}`, data);
    return response.data;
  },

  // Resolve issue
  resolveIssue: async (id: number): Promise<Issue> => {
    const response = await apiClient.patch<Issue>(`/issues/${id}/resolve`);
    return response.data;
  },

  // Delete issue
  deleteIssue: async (id: number): Promise<void> => {
    await apiClient.delete(`/issues/${id}`);
  },
};
