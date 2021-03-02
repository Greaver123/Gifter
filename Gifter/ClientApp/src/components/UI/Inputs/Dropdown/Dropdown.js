import React, { Component } from 'react';
import classes from './Dropdown.module.css';
class Dropdown extends Component {
  state = {
    options: [{ id: 0, value: 'None' }, ...this.props.items],
    selectedId: 0,
    open: false,
  };

  selectOption = (id) => {
    this.setState({ selectedId: id, open: false });
  };

  openOptions = () => {
    this.setState({ open: true });
  };

  closeOptions = (e) => {
    if (
      !e.target.classList.contains(classes.Option) ||
      e.target.classList.contains(classes.Opened)
    ) {
      this.setState({ open: false });
    }
  };

  componentDidMount() {
    window.addEventListener('click', this.closeOptions);
  }

  componentWillUnmount() {
    window.removeEventListener('click', this.closeOptions);
  }

  render() {
    const selectedOption = this.state.options.find(
      (option) => option.id === this.state.selectedId
    );

    let options = [
      <div
        key={-1}
        className={[
          classes.Option,
          classes.Selected,
          this.state.open ? classes.Opened : classes.Closed,
        ].join(' ')}
        onClick={this.closeOptions}
      >
        {selectedOption.value}
      </div>,
    ];

    let selectOptions = [];

    if (this.state.open) {
      this.state.options.forEach((option, index) => {
        options.push(
          <div
            className={[
              classes.Option,
              this.state.selectedId === option.id ? classes.Selected : null,
            ].join(' ')}
            key={option.id}
            data-option-id={option.id}
            onClick={() => {
              this.selectOption(option.id, option.value);
            }}
          >
            {option.value}
          </div>
        );

        selectOptions.push(
          <option key={option.id} value={option.id}>
            {option.value}
          </option>
        );
      });
    } else {
      options = (
        <div
          data-option-id={selectedOption.id}
          className={[classes.Option, classes.Selected, classes.Closed].join(
            ' '
          )}
          onClick={this.openOptions}
        >
          {selectedOption.value}
        </div>
      );

      selectOptions.push(
        <option key={selectedOption.id} value={selectedOption.value}>
          {selectedOption.value}
        </option>
      );
    }

    return (
      <div className={classes.Dropdown}>
        {options}
        <select value={this.state.selectedId} onChange={() => {}}>
          {selectOptions}
        </select>
      </div>
    );
  }
}

export default Dropdown;
