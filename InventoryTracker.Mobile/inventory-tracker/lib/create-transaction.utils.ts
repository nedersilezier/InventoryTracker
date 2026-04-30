import { CreateTransactionForm, CreateTransactionRequest, TransactionTypeKey, TransactionTypeValue } from './create-transaction.types';

// Maps UI-friendly transaction type keys to numeric enum values expected by the API.
export function toTransactionTypeValue(type: TransactionTypeKey) {
  switch (type) {
    case 'issue':
      return TransactionTypeValue.IssueToClient;
    case 'return':
      return TransactionTypeValue.ReturnFromClient;
    case 'transfer':
      return TransactionTypeValue.TransferBetweenWarehouses;
    case 'adjustment':
      return TransactionTypeValue.Adjustment;
  }
}

// Returns human-readable labels used in the mobile UI.
export function getTypeLabel(type: TransactionTypeKey) {
  switch (type) {
    case 'issue':
      return 'Issue';
    case 'return':
      return 'Return';
    case 'transfer':
      return 'Transfer';
    case 'adjustment':
      return 'Adjustment';
  }
}

// Defines how "From / To" UI fields should behave depending on selected transaction type.
export function getRouteLabels(type: TransactionTypeKey) {
  switch (type) {
    case 'issue':
      return {
        showFrom: true,
        fromLabel: 'From warehouse',
        toLabel: 'To client',
      };
    case 'return':
      return {
        showFrom: true,
        fromLabel: 'From client',
        toLabel: 'To warehouse',
      };
    case 'transfer':
      return {
        showFrom: true,
        fromLabel: 'From warehouse',
        toLabel: 'To warehouse',
      };
    case 'adjustment':
      return {
        showFrom: false,
        fromLabel: '',
        toLabel: 'Warehouse',
      };
  }
}

// Formats date for display only
export function formatDisplayDate(date: Date) {
  return new Intl.DateTimeFormat('en-GB', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  }).format(date);
}

// Determines whether quantity must be a whole number for a given unit of meaasure
export function isIntegerUnit(unitOfMeasure: string) {
  const value = String(unitOfMeasure).toLowerCase();

  return (
    value === 'pcs'
  );
}

// Converts mobile form state into the request shape expected by the api(reshapes fromid, toid, items)
export function mapFormToCreateRequest(form: CreateTransactionForm): CreateTransactionRequest {
  const request: CreateTransactionRequest = {
    type: toTransactionTypeValue(form.type),
    transactionDate: new Date().toISOString(),
    clientId: null,
    sourceWarehouseId: null,
    destinationWarehouseId: null,
    referenceNumber: form.referenceNumber.trim() || null,
    notes: form.notes.trim() || null,
    items: form.items.map((item) => ({
      itemId: item.itemId,
      quantity: item.quantity,
    })),
  };
  if (form.type === 'issue') {
    request.sourceWarehouseId = form.fromId ?? null;
    request.clientId = form.toId ?? null;
  }

  if (form.type === 'return') {
    request.clientId = form.fromId ?? null;
    request.destinationWarehouseId = form.toId ?? null;
  }

  if (form.type === 'transfer') {
    request.sourceWarehouseId = form.fromId ?? null;
    request.destinationWarehouseId = form.toId ?? null;
  }

  if (form.type === 'adjustment') {
    request.sourceWarehouseId = form.toId ?? null;
  }

  return request;
}