import React, {ChangeEvent, useState} from 'react';
import {Form, Segment, Button} from 'semantic-ui-react';
import {Author, Book} from "../../../app/models/book";
import {useStore} from "../../../app/stores/store";

interface Props{
    author: Author;
    createOrEdit: (book: {
        pages: number;
        author: {
            id: string,
            firstName: string,
            lastName: string
        };
        price: number;
        isbn: string;
        name: string;
        id: string;
        publishedYear: number;
        authorId: string;
        stocks: number }) => void;
    submitting: boolean;
    changeAuthor: (name: string, value: string) => void;
}

export default function BookForm({createOrEdit}: Props){
    const {bookStore} = useStore();
    const {submitting, selectedBook, closeForm, setAuthor, author, handleCreateOrEditBook} = bookStore;
    function handleSubmit(){
        handleCreateOrEditBook(book);
    }

    //to track the initial state of the form
    const initialState = selectedBook ?? {
        id: "",
        isbn: '',
        name: '',
        authorId: '1',
        author: author,
        pages: 0,
        price: 0,
        stocks: 0,
        publishedYear: 2000,
    }

    const [book, setBook] = useState(initialState);


    async function handleOnChange(e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        let {name, value} = e.target;

        setBook({...book, [name]: value});
    }

    return (
        <Segment clearing>
            <Form onSubmit={handleSubmit} autoComplete='off'>
                <Form.Input placeholder='ISBN' value={book.isbn} name='isbn' onChange={handleOnChange}/>
                <Form.Input placeholder='Title' value={book.name} name='name' onChange={handleOnChange}/>
                <Form.Input placeholder={author.firstName} name='firstName' onChange={(e) => setAuthor('firstName', e.target.value)}/>
                <Form.Input placeholder={author.lastName} name='lastName' onChange={(e) => setAuthor('lastName', e.target.value)}/>
                <Form.Input placeholder='Pages' type='number' value={book.pages} name='pages' onChange={handleOnChange}/>
                <Form.Input placeholder='Price' type='number' value={book.price} name='price' onChange={handleOnChange}/>
                <Form.Input placeholder='Stocks' type='number' value={book.stocks} name='stocks' onChange={handleOnChange}/>
                <Form.Input placeholder='Published Year' type='number' value={book.publishedYear} name='publishedYear' onChange={handleOnChange}/>
                <p className="bio">Close this panel to see a book's details</p>
                <Button loading={submitting} floated='right' positive type='submit' content='Submit'/>
                <Button onClick={closeForm} floated='right' type='submit' content='Cancel'/>
            </Form>
        </Segment>
    )
}
