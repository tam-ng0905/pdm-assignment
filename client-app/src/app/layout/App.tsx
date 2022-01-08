import React, {useEffect, useState} from 'react';
import {Container} from 'semantic-ui-react';
import {Author, Book} from "../models/book";
import BookDashboard from "../../features/books/dashboard/BookDashboard";
import NavBar from "./NavBar";
import {v4 as uuid} from 'uuid';
import agent from '../api/agent';
import LoadingComponent from "./LoadingComponent";
import {useStore} from "../stores/store";
import {observer} from "mobx-react-lite";
import LoginForm from "../../features/users/LoginForm";

function App() {


    //Set up all the states
    const {bookStore} = useStore();
    const [selectedBook, setSelectedBook] = useState<Book | undefined>(undefined);
    const [editMode, setEditMode] = useState(false);
    const [books, setBooks] = useState<Book[]>([]);
    const [authors, setAuthors] = useState<Author[]>([]);
    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);

    //Initial the default data for user
    const [author, setAuthor] =  useState({
        id: "123455",
        firstName: "",
        lastName: ""
    });

    useEffect(() => {
        //Load books initially
        bookStore.loadBooks();

        //if a book is selected, get the info of the author for that book
        if (selectedBook) {
            setLoading(true);
            agent.Authors.details(selectedBook['authorId']).then(response => {
                setAuthor(response);
            })
        } else {
            //Set the default author for initial state
            agent.Authors.details('d2af6c4c-4d41-4fb5-92d3-4efe3d5bffee').then(response => {
                setAuthor(response);
            })
        }
        //Monitor changes for the value of selectedBook and bookStore
    }, [selectedBook, bookStore]);


    //Change the value of the author, to use for updating and creating book
    function handleChangeAuthor(name: string, value: string){
        if (name === 'firstName') {
            author.firstName = value;
        } else if(name === 'lastName') {
            author.lastName = value;
        }
    }

    //Use to update or create book
    function handleCreateOrEditBook(book: Book) {

        //setting submitting to true so that we can start `loading indicator`
        setSubmitting(true);

        //if there is an id, it is an Update operation. Otherwise, it's Create
        if(book.id && book.id !== '1'){
            book.author = author;
            agent.Books.update(book).then(() => {
                setBooks([...books.filter(b => b.id !== book.id), book])
                setSelectedBook(book);
                setEditMode(false);
                setSubmitting(false);
            })
        } else {
            //Set id for new book
            book.id = uuid();
            //Set id for the author
            const newId = uuid();
            book.authorId = newId;
            book.author.id = newId;
            book.author.firstName = author.firstName;
            book.author.lastName = author.lastName;
            agent.Books.create(book).then(() => {
                setBooks([...books, book]);
                setSelectedBook(book);
                setEditMode(false);
                setSubmitting(false);
            })
        }
    }

    if (bookStore.loadingInitial) return <LoadingComponent content='Loading app'/>

    return (
        <>
            <NavBar/>
            <Container style={{marginTop: '7em'}}>
                <LoginForm/>
                <BookDashboard
                    books={bookStore.books}
                    authors={authors}
                    author={author}
                    changeAuthor={handleChangeAuthor}
                    createOrEdit={handleCreateOrEditBook}
                    submitting={submitting}
                />
            </Container>
        </>
    );
}

export default observer(App);
