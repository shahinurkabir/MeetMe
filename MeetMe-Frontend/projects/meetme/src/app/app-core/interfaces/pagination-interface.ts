export interface IPaginationInfo {
    pageSize: number;
    pageNumber: number;
    totalRecords: number;
    totalPages: number;
    isLastPage: boolean;
    isFirstPage: boolean;
}