import React, {useState} from 'react';
import {Menu, Container, Button} from 'semantic-ui-react';
import {useStore} from "../stores/store";

//Navbar component


export default function NavBar(){
    const {bookStore} = useStore();

    //Initial the default data for user
    const [author, setAuthor] =  useState({
        id: "123455",
        firstName: "",
        lastName: ""
    });
    return (
        <Menu  fixed='top'>
            <Container>
                <Menu.Item>
                    <Button onClick={() => bookStore.openForm(author)} positive content='Add book'/>
                </Menu.Item>
            </Container>
        </Menu>
    )
}
