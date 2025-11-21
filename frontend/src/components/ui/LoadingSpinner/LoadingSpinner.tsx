export interface LoadingSpinnerProps {
  message?: string;
  size?: 'sm' | 'md' | 'lg';
}

const sizeClasses = {
  sm: 'h-6 w-6 border-2',
  md: 'h-10 w-10 border-4',
  lg: 'h-12 w-12 border-4',
};

export function LoadingSpinner({ message = 'Loading...', size = 'lg' }: LoadingSpinnerProps) {
  const sizeClass = sizeClasses[size];

  return (
    <div className="text-center py-12">
      <div
        className={`inline-block animate-spin rounded-full border-blue-500 border-t-transparent ${sizeClass}`}
      />
      {message && <p className="mt-4 text-gray-600">{message}</p>}
    </div>
  );
}
