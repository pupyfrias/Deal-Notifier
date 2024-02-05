export type PagedCollection<T> =
  {
    href: string;
    next: string | null;
    prev: string | null;
    total: number;
    limit: number;
    offset: number;
    items: T[];
  };
