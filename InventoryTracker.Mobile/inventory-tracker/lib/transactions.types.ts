//transaction dto returned from api
export type TransactionListDTO = {
  transactionId: string;
  type: number;
  typeName: 'Issue' | 'Return' | 'Transfer' | 'Adjustment' | string;
  status: number;
  statusName: string;
  clientId: string | null;
  clientName: string;
  sourceWarehouseId: string | null;
  destinationWarehouseId: string | null;
  sourceWarehouseNameSnapshot: string | null;
  destinationWarehouseNameSnapshot: string | null;
  transactionDate: string;
  referenceNumber: string | null;
  fromDisplay: string | null;
  toDisplay: string | null;
};

//generig paginated response
export type PagedResponse<T> = {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  totalActive?: number;
};
//filters sent to api
export type TransactionFilters = {
  includeReturns: boolean;
  includeIssues: boolean;
  includeTransfers: boolean;
  includeAdjustments: boolean;
  dateFrom?: string;
  dateTo?: string;
  searchTerm?: string;
};
//default transaction filters
export const DEFAULT_FILTERS: TransactionFilters = {
  includeReturns: true,
  includeIssues: true,
  includeTransfers: true,
  includeAdjustments: true,
  searchTerm: undefined,
};
//request body
export type GetTransactionsParams = {
  pageNumber: number;
  pageSize: number;
  includeReturns?: boolean;
  includeIssues?: boolean;
  includeTransfers?: boolean;
  includeAdjustments?: boolean;
  searchTerm?: string;
  dateFrom?: string;
  dateTo?: string;
};