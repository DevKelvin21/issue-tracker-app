import { Button } from '../Button';

export interface PaginationProps {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
  onPageChange: (page: number) => void;
}

export function Pagination({
  currentPage,
  totalPages,
  pageSize,
  totalCount,
  hasPrevious,
  hasNext,
  onPageChange,
}: PaginationProps) {
  const startItem = (currentPage - 1) * pageSize + 1;
  const endItem = Math.min(currentPage * pageSize, totalCount);

  const handlePrevious = () => {
    if (hasPrevious) {
      onPageChange(currentPage - 1);
    }
  };

  const handleNext = () => {
    if (hasNext) {
      onPageChange(currentPage + 1);
    }
  };

  return (
    <div className="flex items-center justify-between">
      <div className="text-sm text-gray-700">
        Showing <span className="font-medium">{startItem}</span> - <span className="font-medium">{endItem}</span> of{' '}
        <span className="font-medium">{totalCount}</span> results
      </div>

      <div className="flex items-center gap-2">
        <Button variant="secondary" size="sm" onClick={handlePrevious} disabled={!hasPrevious}>
          Previous
        </Button>

        <div className="px-4 py-2 text-sm font-medium text-gray-700">
          Page {currentPage} of {totalPages}
        </div>

        <Button variant="secondary" size="sm" onClick={handleNext} disabled={!hasNext}>
          Next
        </Button>
      </div>
    </div>
  );
}
