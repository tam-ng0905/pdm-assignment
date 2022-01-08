import React, {useEffect, useState} from 'react';
import {Segment, Card, Icon, Image, Button} from 'semantic-ui-react';
import {useStore} from "../../../app/stores/store";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import agent from "../../../app/api/agent";


interface Author{
    id: string,
    firstName: string,
    lastName: string
}


export default function BookDetail(){
    const {bookStore} = useStore();
    const {openForm, cancelSelectBook, selectedBook, setLoading} = bookStore;


    //Set the initial state for the author object
    const [author, setAuthor] =  useState({
        id: "123455",
        firstName: "",
        lastName: ""
    });

    useEffect(() => {

        //if a book selected, return the author's details
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
    }, [selectedBook, bookStore]);

    //if no book selected, show loading component
    if (!selectedBook) return <LoadingComponent />;

    return (
        <Segment clearing>
            <Image src={`/assets/images/books-unsplash.jpeg`}/>
            <br/>
            <Card.Content>
                <Card.Header>{selectedBook.name}</Card.Header>
                <br/>
                {/*If there is no author, return nothing. Otherwise, returns first name and last name*/}
                {author ?
                    <Card.Meta>
                        <span className='date'><Icon name='user' /> {author.firstName} {author.lastName}
                        </span>
                    </Card.Meta>
                    :
                        ""
                }
                <Card.Description>
                    Stocks: {selectedBook.stocks}
                </Card.Description>
                <Card.Description>
                    Pages: {selectedBook.pages}
                </Card.Description>
                <Card.Description>
                    Published Year: {selectedBook.publishedYear}
                </Card.Description>
                <Card.Description>
                    Price: {selectedBook.price}
                </Card.Description>
            </Card.Content>
            <Card.Content extra>
                <Button.Group widths='2'>
                    <Button onClick={() => openForm(author, selectedBook)} basic color='grey' content="Edit"></Button>
                    <Button onClick={cancelSelectBook}basic color='grey' content="Cancel"></Button>
                </Button.Group>
            </Card.Content>
        </Segment>
    )
}
