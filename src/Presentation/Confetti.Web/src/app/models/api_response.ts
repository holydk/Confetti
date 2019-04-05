export class ApiResponse<T> {
    public successful: boolean;
    public errors: string[];
    public response: T;
}
