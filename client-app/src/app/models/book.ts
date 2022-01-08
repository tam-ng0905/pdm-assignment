//The interface based on the Author's schema from the backend
export interface Author {
    id: string,
    firstName: string,
    lastName: string,
}

//The interface based on the Title's schema from the backend
export interface Book {
    id: string;
    isbn: string;
    name: string;
    authorId: string,
    author: Author,
    publishedYear: number;
    pages: number;
    price: number;
    stocks: number;
}
