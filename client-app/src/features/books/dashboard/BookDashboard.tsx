import React, {useEffect, useState} from 'react';
import {Grid, Button} from 'semantic-ui-react';
import {Author, Book} from '../../../app/models/book';
import BookList from "./BookList";
import BookForm from "../form/BookForm";
import BookDetail from "../detail/BookDetail";
import {useStore} from "../../../app/stores/store";
import {observer} from "mobx-react-lite";
import {PagingParams} from "../../../app/models/pagination";
import LoadingComponent from '../../../app/layout/LoadingComponent';

//set up the props for the component
interface Props {
    books: Book[];
    authors: Author[];
    author: Author;
    changeAuthor: (name: string, value: string) => void;
    createOrEdit: (book: Book) => void;
    submitting: boolean;
}

export default observer( function BookDashboard({
        books, author, createOrEdit, submitting, changeAuthor}: Props) {

    const {bookStore} = useStore();

    //get states from the store
    const {query, loadBooks, selectedBook, editMode, setPrice, setQuery, setSearch, setPagingParams, pagination} = bookStore;
    const [loadingNext, setLoadingNext] = useState(false);


    //use to fetch more book per page
    function handleGetNext(){
        setLoadingNext(true);
        setPagingParams(new PagingParams(pagination!.currentPage + 1))
        window.scrollTo(0, 0);
        if(query){
            setSearch().then(() => setLoadingNext(false));
        } else {
            loadBooks().then(() => setLoadingNext(false));
        }
    }

    if(bookStore.loadingInitial && !loadingNext){
        return <LoadingComponent content='Loading books'/>
    }

    return (

        <div>
            <div className="mainContainer">
                <div className="dataContainer">
                    <div className="header">
                        University Bookstore 👋
                    </div>

                    <div className="bio">
                        I am Tam and this is my interview project for PDM
                    </div>
                    <br/>
                    <input placeholder="Search with a book's name" className="box"
                           style={{boxShadow: "0.5px 2.5px 0.5px lightgrey"}}
                           onChange={(e) => {setQuery(e.target.value)}}
                    />
                    <input type="number" placeholder="Maximum price" className="box"
                           style={{boxShadow: "0.5px 2.5px 0.5px lightgrey"}}
                           onChange={(e) => {
                               setPrice(parseInt(e.target.value))
                           }}
                    />

                    <button onClick={() => setSearch()} className="box">
                        Search
                    </button>

                    <br/>
                </div>
            </div>
            <Grid>
                <Grid.Column width='10'>
                    {/*Show list of books*/}
                    <BookList books={books}/>
                    <Button floated='right' content='More...' positive onClick={handleGetNext} loading={loadingNext} disable={pagination?.totalPages === pagination?.currentPage}></Button>
                </Grid.Column>
                <Grid.Column width='6'>
                    {/*if the user click detail, and not in the edit mode, show the book detail panel*/}
                    {selectedBook && !editMode &&
                        <BookDetail/>
                    }
                    {/*If the user in edit mode, show book form*/}
                    {editMode &&
                        <BookForm
                                  changeAuthor={changeAuthor}
                                  // authors={authors}
                                  author={author}
                                  createOrEdit={createOrEdit}
                                  submitting={submitting}
                        />}
                </Grid.Column>
            </Grid>

        </div>
    )
})
