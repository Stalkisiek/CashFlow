export class BankAccount {
    id: number;
    type: number;
    name: string;
    balance: number;
    creditBalance: number;
    createdAt: string;
    updatedAt: string;

    constructor(
        id: number,
        type: number,
        name: string,
        balance: number,
        creditBalance: number,
        createdAt: string,
        updatedAt: string
    ) {
        this.id = id;
        this.type = type;
        this.name = name;
        this.balance = balance;
        this.creditBalance = creditBalance;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
    }
}
