import { useCallback } from 'react';

export interface UseConfirmDialogReturn {
  confirm: (message: string) => Promise<boolean>;
}

/**
 * Hook for showing confirmation dialogs.
 * Currently uses window.confirm, but can be extended to use a custom modal.
 */
export function useConfirmDialog(): UseConfirmDialogReturn {
  const confirm = useCallback(async (message: string): Promise<boolean> => {
    return window.confirm(message);
  }, []);

  return { confirm };
}
