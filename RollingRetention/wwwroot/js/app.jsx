class Activity extends React.Component {

    constructor(props)
    {
        super(props);
        this.state = { data: props.activity };
        this.onClick = this.onClick.bind(this);
    }
    onClick(e)
    {
        this.props.onRemove(this.state.data);
    }
    render()
    {
        return
        <div>
            <p><b>{this.state.data.name}</b></p>
            <p>Цена {this.state.data.price}</p>
            <p><button onClick={this.onClick}>Удалить</button></p>
        </div>;
    }
}

class Form extends React.Component {

    constructor(props) {
        super(props);
        this.state = { userId: 0, registrationDate: "", lastActivityDate: "" };
        this.onSubmit = this.onSubmit.bind(this);
        this.onUserIdChange = this.onUserIdChange.bind(this);
        this.onRegistrChange = this.onRegistrChange.bind(this);
        this.onLastChange = this.onLastChange.bind(this);
    }
    onUserIdChange(e) {
        this.setState({ userId: e.target.value });
    }
    onRegistrChange(e) {
        this.setState({ registr: e.target.value });
    }
    onLastChange(e) {
        this.setState({ last: e.target.value });
    }

    onSubmit(e) {
        e.preventDefault();
        var actUserID = this.state.userId;
        var actRegistr = this.state.registr;
        var actLast = this.state.last;
        if (!actUserID || actRegistr == "" || actLast == "" ) {
            return;
        }
        this.props.onSubmit({ userId: actUserID, registr: actRegistr, last: actLast});
        this.setState({ userId: "", registr: "" , last: "" });
    }
    
    render() {
        return (
            < form onSubmit={this.onSubmit} >
                <p>
                    <input type="number"
                        placeholder="User Id"
                        value={this.state.userId}
                        onChange={this.onUserIdChange} />

                    <input type="date"
                        placeholder="Date Registration"
                        value={this.state.registr}
                        onChange={this.onRegistrChange} />

                    <input type="date"
                        placeholder="Date Last Activity"
                        value={this.state.last}
                        onChange={this.onLastChange} />
                </p>

                <input type="submit" value="Save" />
            </form>
        );
    }
}


class InputTable extends React.Component {

    constructor(props) {
        super(props);
        this.state = { activitys: [] };

        this.onAdd = this.onAdd.bind(this);
        this.onRemove = this.onRemove.bind(this);
    }

    // Load data
    loadData() {
        var xhr = new XMLHttpRequest();
        xhr.open("GetUserActivitys", this.props.apiUrl, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ activitys: data });
        }.bind(this);
        xhr.send();
    }
    componentDidMount() {
        this.loadData();
    }

    // Add object
    onAdd(activitys) {
        if (activitys) {

            const data = new FormData();
            data.append("userId", activitys.userId);
            data.append("registrationDate", activitys.registr);
            data.append("lastActivityDate", activitys.last);
            console.log(data);
            var xhr = new XMLHttpRequest();

            xhr.open("post", this.props.apiUrl, true);
            xhr.onload = function () {
                if (xhr.status === 200) {
                    this.loadData();
                }
            }.bind(this);
            xhr.send(data);
        }
    }
}

ReactDOM.render(
    <InputTable apiUrl="/api/UserActivitys" />,
    document.getElementById("app")
);
