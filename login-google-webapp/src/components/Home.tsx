import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import { useDispatch, useSelector } from 'react-redux';
import { setAuth } from '../redux/authSlice';
import { RootState } from '../redux/store';

const Home = () => {
    const dispatch = useDispatch();
    const [mensagem, setMensagem] = useState('Você não está logado');
    const auth = useSelector((state: RootState) => state.auth.value);

    useEffect(() => {
        (async () => {
            try {
                const {data} = await axios.get('user');
               
                setMensagem(`Olá ${data.first_name} ${data.last_name}`);
                dispatch(setAuth(true));
            } catch (error) {
                setMensagem(`Você não está logado`);
                dispatch(setAuth(false));
            }
        })();
    },[]);

    return (
        <div className='container mt-5 text-center'>
            <h3>{auth ? mensagem : "Você não está logado"}</h3>
        </div>
    );
}

export default Home;