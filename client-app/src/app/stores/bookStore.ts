import {makeAutoObservable, runInAction} from "mobx";
import {Author, Book} from "../models/book";
import agent from "../api/agent";
import {Pagination, PagingParams} from "../models/pagination";
import {v4 as uuid} from 'uuid';


//Central store to manage all the states for client app


export default class BookStore {


    //Set up all the initial state
    pagination: Pagination | null = null;
    books: Book[] = [];
    selectedBook: Book | undefined = undefined;
    editMode = false;
    loading = true;
    didSearch = false;
    previousQuery: string = '';
    query: string = '';
    price: number = 0;
    submitting = false;
    author: Author = {
        id: "12345",
        firstName: "Tam",
        lastName: ""
    };
    loadingInitial = false;
    pagingParams = new PagingParams();



    constructor(){
        makeAutoObservable(this)
    }

    //Set up the initial params for pagination
    setPagingParams = (pagingParams: PagingParams) => {
        this.pagingParams = pagingParams;
    }


    //Embed the pageNumber and pageSize for search query by default
    get axiosParams() {
        const params = new URLSearchParams();
        params.append('pageNumber', this.pagingParams.pageNumber.toString());
        params.append('pageSize', this.pagingParams.pageSize.toString());
        return params;
    }

    setLoading = (state: boolean) => {
        this.loading = state;
    }


    //change the author with initial author object
    setAuthorWithObject = async (author: Author) => {
        this.author = author
    }


    //change the author first name and last name
    setAuthor = (name: string, value: string) => {
        if (name === 'firstName') {
            this.author.firstName = value;
        } else if(name === 'lastName') {
            this.author.lastName = value;
        }
    }


    selectBook = async (id: string) => {
        this.selectedBook = this.books.find(a => a.id === id);
    }

    handleCreateOrEditBook = (book: Book) => {

        //setting submitting to true so that we can start `loading indicator`
        this.setSubmitting(true);

        //if there is an id, it is an Update operation. Otherwise, it's Create
        if(book.id && book.id != '1'){
            book.author = this.author;
            agent.Books.update(book).then(() => {
                this.setBooks([book, ...this.books.filter(b => b.id !== book.id)])
                this.setSelectedBook(book);
                this.setEditMode(false);
                this.setSubmitting(false);
            })
        } else {
            book.id = uuid();
            const newId = uuid();
            book.authorId = newId;
            book.author.id = newId;
            agent.Books.create(book).then(() => {
                this.setBooks([book, ...this.books]);
                this.setSelectedBook(book);
                this.setEditMode(false);
                this.setSubmitting(false);
            })
        }
    }

    setSelectedBook = async (book: Book) => {
        this.selectedBook = book;
    }

    //detect when the user open the panel to update or create book
    setEditMode = (mode: boolean) => {
        this.editMode = mode;
    }

    setSubmitting = (state: boolean) => {
        this.submitting = state;
    }

    //feed the client with seed books from the server
    loadBooks = async () => {
        this.setLoadingInitial(true);
        try {
            if(!this.didSearch) {
                const results = await agent.Books.seed(this.axiosParams);
                this.setLoadingInitial(false);
                this.setPagination(results.pagination);
                this.setBooks(results.data);
            }

        } catch(error) {
            console.log(error);
            runInAction(() => {
                this.setLoadingInitial(false);
            })
        }
    }

    setPagination = (pagination: Pagination) => {
        this.pagination = pagination;
    }


    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }


    //set the search query
    setQuery = (q: string) => {
        this.query = q
    }

    //set the previously used query to reset the pagination values
    setPrevQuery = (q: string) => {
        this.previousQuery = q
}

    setPrice = async (p: number) => {
        this.price = p
    }

    setBooks = (b: Book[]) => {
        this.books = b;
    }

    setSearch = async () => {

        //reset the pagination value if new query is used
        if(this.query != this.previousQuery){
            this.setPagingParams(new PagingParams(1))
        }

        //if the query is put in, search. Otherwise, feed the client with seed books
        if(this.query){
            const results = await agent.Books.search(this.query, this.price, this.axiosParams);
            this.setPagination(results.pagination);
            this.setBooks(results.data);
        } else {
            const results = await agent.Books.seed(this.axiosParams);
            this.setPagination(results.pagination);
            this.setBooks(results.data);
            this.setLoading(false);
        }
        this.setDidSearch(true);
        this.setPrevQuery(this.query);
    }

    setDidSearch = (state: boolean) => {
        this.didSearch = state;
    }


    cancelSelectBook = () => {
        this.selectedBook = undefined;
    }

    openForm = async (author: Author, book?: Book ) => {
        book ? this.selectBook(book.id) : this.cancelSelectBook();
        this.editMode = true;
        this.setAuthorWithObject(author);

    }

    setDeleteBook = (id: string) => {
        agent.Books.delete(id).then(() => {
            this.setBooks([...this.books.filter(b => b.id !== id)])
        })
    }

    closeForm = () => {
        this.editMode = false;
    }
}
