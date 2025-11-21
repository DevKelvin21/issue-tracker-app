import { Select } from '../../../../components/ui';
import { IssueStatus } from '../../../../types/domain';
import { getStatusLabel } from '../../../../lib/utils';

export interface IssueFiltersProps {
  statusFilter: IssueStatus | undefined;
  pageSize: number;
  onStatusChange: (status: IssueStatus | undefined) => void;
  onPageSizeChange: (size: number) => void;
}

export function IssueFilters({ statusFilter, pageSize, onStatusChange, onPageSizeChange }: IssueFiltersProps) {
  const handleStatusChange = (value: string) => {
    onStatusChange(value ? (Number(value) as IssueStatus) : undefined);
  };

  const handlePageSizeChange = (value: string) => {
    onPageSizeChange(Number(value));
  };

  return (
    <div className="flex items-center gap-6">
      <div className="flex items-center gap-2">
        <label htmlFor="status-filter" className="text-sm font-medium text-gray-700">
          Filter by status:
        </label>
        <Select
          id="status-filter"
          value={statusFilter ?? ''}
          onChange={(e) => handleStatusChange(e.target.value)}
          className="text-sm"
        >
          <option value="">All Issues</option>
          <option value={IssueStatus.Open}>{getStatusLabel(IssueStatus.Open)}</option>
          <option value={IssueStatus.InProgress}>{getStatusLabel(IssueStatus.InProgress)}</option>
          <option value={IssueStatus.Resolved}>{getStatusLabel(IssueStatus.Resolved)}</option>
        </Select>
      </div>

      <div className="flex items-center gap-2">
        <label htmlFor="page-size" className="text-sm font-medium text-gray-700">
          Items per page:
        </label>
        <Select id="page-size" value={pageSize} onChange={(e) => handlePageSizeChange(e.target.value)} className="text-sm">
          <option value="10">10</option>
          <option value="20">20</option>
          <option value="50">50</option>
          <option value="100">100</option>
        </Select>
      </div>
    </div>
  );
}
