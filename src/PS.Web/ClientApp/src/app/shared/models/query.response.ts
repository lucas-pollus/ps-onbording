export interface QueryResponse<T> {
  page: number;
  pageSize: number;
  totalRows: number;
  totalPages: number;
  hasNextPage: boolean;
  values: T[];
}
