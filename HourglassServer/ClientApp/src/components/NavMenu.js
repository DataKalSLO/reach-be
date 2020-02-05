import React from "react";
import {
   Collapse,
   Container,
   Navbar,
   NavbarBrand,
   NavbarToggler,
   NavItem,
   NavLink
} from "reactstrap";
import { Link } from "react-router-dom";
import "./NavMenu.css";
import InputBase from "@material-ui/core/InputBase";
import { fade, makeStyles } from "@material-ui/core/styles";
import SearchIcon from "@material-ui/icons/Search";
import { withStyles } from "@material-ui/core/styles";

const styles = theme => ({
   search: {
      position: "relative",
      borderRadius: theme.shape.borderRadius,
      border: "black",
      backgroundColor: fade(theme.palette.common.white, 0.15),
      "&:hover": {
         backgroundColor: fade(theme.palette.common.white, 0.25)
      },
      marginLeft: 0,
      width: "100%",
      [theme.breakpoints.up("sm")]: {
         marginLeft: theme.spacing(1),
         width: "auto"
      }
   },
   searchIcon: {
      width: theme.spacing(7),
      height: "100%",
      position: "absolute",
      pointerEvents: "none",
      display: "flex",
      alignItems: "center",
      justifyContent: "center"
   },
   inputRoot: {
      color: "inherit"
   },
   inputInput: {
      padding: theme.spacing(1, 1, 1, 7),
      transition: theme.transitions.create("width"),
      width: "100%",
      [theme.breakpoints.up("sm")]: {
         width: 120,
         "&:focus": {
            width: 200
         }
      }
   }
});

class NavMenu extends React.Component {
   constructor(props) {
      super(props);

      this.toggle = this.toggle.bind(this);
      this.state = {
         isOpen: false
      };
   }
   toggle() {
      this.setState({
         isOpen: !this.state.isOpen
      });
   }

   render() {
      const { classes } = this.props;
      return (
         <header>
            <Navbar
               className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3"
               light
            >
               <Container>
                  <NavbarBrand tag={Link} to="/">
                     Hourglass
                  </NavbarBrand>
                  <NavbarToggler onClick={this.toggle} className="mr-2" />
                  <Collapse
                     className="d-sm-inline-flex flex-sm-row-reverse"
                     isOpen={this.state.isOpen}
                     navbar
                  >
                     <ul className="navbar-nav flex-grow">
                        <NavItem>
                           <NavLink tag={Link} className="text-dark" to="/">
                              Initiatives
                           </NavLink>
                        </NavItem>
                        <NavItem>
                           <NavLink tag={Link} className="text-dark" to="/map">
                              Map
                           </NavLink>
                        </NavItem>
                        <NavItem>
                           <NavLink tag={Link} className="text-dark" to="/">
                              Contact Us
                           </NavLink>
                        </NavItem>
                        <NavItem>
                           <div className={classes.search}>
                              <div className={classes.searchIcon}>
                                 <SearchIcon />
                              </div>
                              <InputBase
                                 placeholder="Search…"
                                 classes={{
                                    root: classes.inputRoot,
                                    input: classes.inputInput
                                 }}
                                 inputProps={{ "aria-label": "search" }}
                              />
                           </div>
                        </NavItem>
                     </ul>
                  </Collapse>
               </Container>
            </Navbar>
         </header>
      );
   }
}

export default withStyles(styles)(NavMenu);
