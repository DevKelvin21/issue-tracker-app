import type { HTMLAttributes, ReactNode } from 'react';

export interface CardProps extends HTMLAttributes<HTMLDivElement> {
  children: ReactNode;
  padding?: 'none' | 'sm' | 'md' | 'lg';
}

const paddingClasses = {
  none: '',
  sm: 'p-4',
  md: 'p-6',
  lg: 'p-8',
};

export function Card({ children, padding = 'md', className = '', ...props }: CardProps) {
  const baseClasses = 'bg-white rounded-lg shadow-md border border-gray-200';
  const paddingClass = paddingClasses[padding];

  return (
    <div className={`${baseClasses} ${paddingClass} ${className}`} {...props}>
      {children}
    </div>
  );
}
