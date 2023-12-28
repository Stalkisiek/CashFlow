export class User{
    id: number;
    name: string;
    surname: string;
    email: string;
    authorizationLevel: number;
    createdAt: string;
    updatedAd: string;

    constructor(id: number, name: string, surname: string, email: string, authorizationLevel: number, createdAt: string, updatedAt: string) {
        this.id = id;
        this.name = name;
        this.surname = surname;
        this.email = email;
        this.authorizationLevel = authorizationLevel;
        this.createdAt = createdAt;
        this.updatedAd = updatedAt;
    }
}