import react = require("react");

import React from 'react'
import classes from './DeleteView.module.css'

const DeleteView = (props) => {
    return (
        <div className={classes.DeleteView}>
            {props.children}
        </div>
    )
}

export default DeleteView