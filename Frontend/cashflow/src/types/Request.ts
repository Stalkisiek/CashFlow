export class Request {
    id: number;
    userId: number;
    type: number;
    accountBalance: number;
    amountBalance: number;
    finallBalance: number;
    accountCredit: number;
    amountCredit: number;
    finallCredit: number;

    constructor(id: number, userId: number, type: number, accountBalance: number, amountBalance: number, finallBalance: number, accountCredit: number, amountCredit: number, finallCredit: number) {
        this.id = id;
        this.userId = userId;
        this.type = type;
        this.accountBalance = accountBalance;
        this.amountBalance = amountBalance;
        this.finallBalance = finallBalance;
        this.accountCredit = accountCredit;
        this.amountCredit = amountCredit;
        this.finallCredit = finallCredit;
    }
}
