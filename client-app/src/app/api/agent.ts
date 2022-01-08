// **
// This file contains all the requests to the server
//


import axios, {AxiosResponse} from 'axios';
import {Book, Author} from "../models/book";
import {PaginatedResult} from "../models/pagination";
import {User, UserFormValues} from "../models/user";
import {store} from "../stores/store";

//Use to create the loading indicators for the website
const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay);
    })
}

//Setting up the default URL
axios.defaults.baseURL = 'http://localhost:5000/api';
//send the token in the headers
axios.interceptors.request.use(config => {
    //save and send the jwt token with the requests
    const token = store.commonStore.token;
    if (token) { // @ts-ignore
        config.headers.Authorization = `Bearer ${token}`
    }
    return config;
})
//generate the loading effect
axios.interceptors.response.use(async response => {
    try {
        await sleep(1000);
        const pagination = response.headers['pagination'];
        if (pagination) {
            response.data = new PaginatedResult(response.data, JSON.parse(pagination));
            return response as AxiosResponse<PaginatedResult<any>>
        }
        return response;
    } catch (error) {
        console.log(error);
        return Promise.reject(error);
    }
});

const responseBody = (response: AxiosResponse) => response.data;

// mapping all the request to its response's data
const requests = {
    get: <T> (url: string) => axios.get<T>(url).then(responseBody),
    post: <T> (url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T> (url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del: <T> (url: string) => axios.delete<T>(url).then(responseBody),
}

//map out the requests for all books routes
const Books = {

    //feed the client with 50 books initially
    seed: (params: URLSearchParams) => axios.get<PaginatedResult<Book[]>>('/titles/seed', {params}).then(responseBody),
    //get all the books
    list: () => requests.get<PaginatedResult<Book[]>>('/titles'),
    //search request
    search: (name: string, price: number, params: URLSearchParams) => axios.get<PaginatedResult<Book[]>>(`/titles/search/?query=${name}&price=${price}`, {params}).then(responseBody),
    //get request to show the details of each books
    details: (id: string) => requests.get<Book>(`/titles/${id}`),

    create: (book: Book) => axios.post<void>('/titles', book),
    update: (book: Book) => axios.put<void>(`/titles/${book.id}`, book),
    delete: (id: string) => axios.delete<void>(`/titles/${id}`),
}
//map out the requests for all authors routes
const Authors = {
    //get all authors
    list: () => requests.get<Author[]>('/authors'),
    //get the First Name and Last Name of the author
    details: (id: string) => requests.get<Author>(`/authors/${id}`),

    create: (author: Author) => axios.post<void>('/authors', author),
    update: (author: Author) => axios.put<void>(`/authors/${author.id}`, author),
    delete: (id: string) => axios.delete<void>(`/authors/${id}`)
}

const Account = {
    //get the current user
    current: () => requests.get<User>('/account'),
    login: (user: UserFormValues) => requests.post<User>('/account/login', user),
    refreshToken: () => requests.post<User>('/account/refreshToken', {}),
}

const agent = {
    Books,
    Authors,
    Account
}
export default agent;
