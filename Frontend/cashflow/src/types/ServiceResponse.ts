export class ServiceResponse<T> {
    data: T;
    success: boolean;
    statusCode: number;
    message: string;

    constructor(data: T, success: boolean, statusCode: number, message: string) {
        this.data = data;
        this.success = success;
        this.statusCode = statusCode;
        this.message = message;
    }
}