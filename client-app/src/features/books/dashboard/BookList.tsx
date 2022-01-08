import React from 'react';
import {Book} from "../../../app/models/book";
import {Segment, Item, Button} from "semantic-ui-react";
import {useStore} from "../../../app/stores/store";


interface Props {
    books: Book[];
}

export default function BookList({books}: Props){
    const {bookStore} = useStore();
    const {setDeleteBook} = bookStore;

    return(
        <Segment className="dataContainer">
            {

                //If there are books returned from the server, show it. Otherwise, return no result
                books.length  > 0 ?
                    <Item.Group divided>
                        {books.map(book => (
                            <Item className="itemContainer" key={book.id} style={{boxShadow: "0.5px 2.5px 0.5px lightgrey", borderRadius: "5px", marginBottom: "20px"}}>
                                <Item.Content>
                                    <Item.Header as='a' className="header">
                                        {book.name}
                                    </Item.Header>
                                    <br/>
                                    <Item.Meta>Year Published: {book.publishedYear}</Item.Meta>
                                    <br/>
                                    <Item.Description className="description">{book.stocks} in stock</Item.Description>
                                    <Item.Extra style={{marginRight: "20px"}}>
                                        <Button onClick={() => setDeleteBook(book.id)} className="box" floated='right' content='Delete' color='grey' />
                                        <Button onClick={() => bookStore.selectBook(book.id)} className="box" floated='right' content='Details' color='orange' />
                                    </Item.Extra>
                                </Item.Content>
                            </Item>
                        ))}
                    </Item.Group>

                    :

                    <h2>No result! Please try another search with different value</h2>
            }
        </Segment>
    )
}
