import React from 'react';
import { Dimmer, Loader } from 'semantic-ui-react';


//Create the loading effect

interface Props {
    inverted?: boolean;
    content?: string;
}

export default function LoadingComponent({inverted = true, content = 'Loading...'}: Props) {
    return (
        <Dimmer active={true} inverted={inverted}>
            <Loader content={content} />
        </Dimmer>
    )
}
