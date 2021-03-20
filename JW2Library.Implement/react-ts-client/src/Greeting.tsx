import React from 'react';

type GreetingsProps = {
    name: string;
    mark: string;
    optional?: string; //exists or nothing
    onClick: (name: string) => void; //return nothing...
};

function Greetings({ name, mark, optional, onClick }: GreetingsProps) {
    const handleClick = () => onClick(name); //define event action;
    return (
        <div>
            Hello, {name} {mark}
            {optional&&<p>{optional}</p>}
            <div>
                <button onClick={handleClick}>Click me</button>
            </div>
        </div>
    );
}

Greetings.defaultProps = {
    mark: '!'
};

export default Greetings;