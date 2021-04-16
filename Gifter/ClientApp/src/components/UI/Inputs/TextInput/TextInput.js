import React, { createRef } from 'react';
import classes from './TextInput.module.css';
import InputToolbar from './InputToolbar/InputToolbar';

class TextInput extends React.Component {
  inputRef = createRef();

  state = {
    toolbar: false,
    isHandlingRequest: false,
    focused: false,
    error: false,
  };

  focusIn = (e) => {
    this.setState({ focused: true });
  };

  focusOut = (e) => {
    if (this.props.value !== '') return;

    setTimeout(() => {
      if (this.state.isHandlingRequest || this.state.error) {
        this.setState({ error: false });
        return;
      }
      this.setState({ focused: false, toolbar: false });
    }, 100);
  };

  showTooblar = () => {
    if (this.state.focused) {
      this.setState({ toolbar: true });
    } else {
      this.inputRef.current.blur();
    }
  };

  save = async () => {
    try {
      if (this.props.value === '') {
        this.setState({ focused: false, toolbar: false });
        return;
      }
      this.inputRef.current.focus();
      this.setState({ isHandlingRequest: true });
      await this.props.onSaveClick();
      this.setState({ focused: false, toolbar: false });
    } catch (ex) {
      alert('Could not save data. Please try again.');
      console.error(ex);
      this.setState({ error: true });
    } finally {
      this.setState({ isHandlingRequest: false });
      this.inputRef.current.focus();
    }
  };

  delete = async () => {
    try {
      if (this.props.value === '') {
        this.setState({ focused: false, toolbar: false });
        return;
      }
      this.inputRef.current.focus();
      this.setState({ isHandlingRequest: true });
      await this.props.onSaveClick();
      this.setState({ focused: false, toolbar: false });
    } catch (ex) {
      alert('Could not Delete data. Please try again.');
      console.error(ex);
      this.setState({ error: true });
    } finally {
      this.setState({ isHandlingRequest: false });
      this.inputRef.current.focus();
    }
  };

  saveOnEnter = async (e) => {
    if (e.key !== 'Enter') return;
    await this.save();
  };

  componentDidMount() {
    // console.dir(this.inputRef.current);
    // this.inputRef.current.parentElement.addEventListener(
    //   'click',
    //   function (e) {
    //     console.log(e.target.closest(`.${classes.TextInput}`));
    //     if (!e.target.closest(`.${classes.TextInput}`)) return;
    //     console.log('click');
    //     this.inputRef.current.focus();
    //     // this.setState({ focused: false, toolbar: false });
    //   }.bind(this)
    // );
    // document.addEventListener(
    //   'keydown',
    //   function (e) {
    //     console.log(e.key);
    //     if (e.key === 'Tab') console.log('tab');
    //     this.setState({ focused: false, toolbar: false });
    //   }.bind(this)
    // );
  }

  render() {
    return (
      <div
        className={[classes.TextInput].join(' ')}
        onMouseDown={this.focusIn}
        onTransitionEnd={this.showTooblar}
      >
        <input
          ref={this.inputRef}
          name={this.props.name}
          placeholder={this.props.placeholder}
          value={this.props.value}
          onChange={this.props.onChange}
          onFocus={this.focusIn}
          onBlur={this.focusOut}
          onKeyDown={this.saveOnEnter}
          className={this.state.focused ? classes.Focused : ''}
          type="text"
          disabled={this.state.isHandlingRequest}
        />
        {
          <InputToolbar
            visible={this.state.toolbar}
            spinner={this.state.isHandlingRequest}
            onSaveClick={this.save.bind(this)}
            onDeleteClick={this.delete.bind(this)}
          />
        }
      </div>
    );
  }
}

export default TextInput;
