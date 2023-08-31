import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import axios from 'axios';
import { useSelector } from 'react-redux';
import { RootState } from '../redux/store';
import { useDispatch } from 'react-redux';
import { setAuth } from '../redux/authSlice';

const Navbar = () => {

    const auth = useSelector((state: RootState) => state.auth.value);
    const dispatch = useDispatch();

    const logout = async () => {
        await axios.post('logout', {}, {withCredentials: true});

        axios.defaults.headers.common['Authorization'] = '';
        dispatch(setAuth(false));
    }

    let links;

    if(auth){
        links =  <div className="text-end">
                    <Link to="/login" onClick={logout} className="btn btn-outline-light me-2">Sair</Link>
                </div>

    }else{
        links =  <div className="text-end">
                    <Link to="/login" className="btn btn-outline-light me-2">Entrar</Link>
                    <Link to="/register" className="btn btn-outline-light me-2">Registrar</Link>
                </div>
    }
    
    return (
        <header className="p-3 text-bg-dark">
            <div className="container">
                <div className="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                        <ul className="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
                            {
                                auth &&
                                    <li><Link to="/" className="nav-link px-2 text-white">Home</Link></li>
                            }
                        </ul>

                    {links}
                </div>
            </div>
        </header>
    )
}

export default Navbar;