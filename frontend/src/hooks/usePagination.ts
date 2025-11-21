import { useState } from 'react';

export interface UsePaginationOptions {
  initialPage?: number;
  initialPageSize?: number;
}

export interface UsePaginationReturn {
  pageNumber: number;
  pageSize: number;
  setPageNumber: (page: number) => void;
  setPageSize: (size: number) => void;
  handlePageChange: (page: number) => void;
  handlePageSizeChange: (size: number) => void;
  resetPagination: () => void;
}

export function usePagination({
  initialPage = 1,
  initialPageSize = 20,
}: UsePaginationOptions = {}): UsePaginationReturn {
  const [pageNumber, setPageNumber] = useState(initialPage);
  const [pageSize, setPageSize] = useState(initialPageSize);

  const handlePageChange = (page: number) => {
    setPageNumber(page);
  };

  const handlePageSizeChange = (size: number) => {
    setPageSize(size);
    setPageNumber(1); // Reset to first page when page size changes
  };

  const resetPagination = () => {
    setPageNumber(initialPage);
    setPageSize(initialPageSize);
  };

  return {
    pageNumber,
    pageSize,
    setPageNumber,
    setPageSize,
    handlePageChange,
    handlePageSizeChange,
    resetPagination,
  };
}
