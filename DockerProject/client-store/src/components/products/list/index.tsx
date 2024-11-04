import { useEffect, useState } from 'react';
import { IProductItem } from '../../../interfaces/products';
import { Link } from 'react-router-dom';
import { Button, Carousel } from 'antd';
import { PlusCircleFilled } from '@ant-design/icons';
import {API_URL, http_common} from "../../../env";
import {DeleteDialog} from "../../common/DeleteDialog.tsx";

const ProductListPage = () => {
    const [list, setList] = useState<IProductItem[]>([]);

    useEffect(() => {
        http_common.get<IProductItem[]>("/api/Products")
            .then(resp => {
                setList(resp.data);
            });
    }, []);

    const handleDelete = async (id: number) => {
        //console.log("Delete id", id);
        try {
            await http_common.delete("/api/products/" + id);
            setList(list.filter(item => item.id !== id));
        } catch {
            //toast
        }
    }

    return (
        <>
            <p className='text-center text-3xl font-bold mb-5'>Products</p>
            <Link to={"/products/create"}>
                <Button type="primary" shape="round" icon={<PlusCircleFilled />} style={{ marginTop: 10, marginBottom: 20 }} />
            </Link>

            <div className='grid md:grid-cols-3 lg:grid-cols-4 gap-10'>
                {list.map(item =>
                    <div key={item.id} className='border rounded-lg overflow-hidden shadow-lg'>
                        <Carousel arrows infinite={false}>
                            {item.images.map((image, i) => (
                                <div key={i}>
                                    <img src={`${API_URL}/images/300_${image}`} alt={item.name}
                                         className='w-full h-48 object-cover'/>
                                </div>
                            ))}
                        </Carousel>
                        <div className='p-4'>
                            <h3 className='text-xl font-semibold mb-2'>{item.name}</h3>
                            <p className='text-teal-800 font-bold text-xl'>{item.price}<span
                                className='text-sm'>$</span></p>

                            <div className='flex justify-between items-center p-2 mt-6'>
                                <DeleteDialog title={"Ви впевнені?"}
                                              description={`Дійсно бажаєте видалити '${item.name}'?`}
                                              onSubmit={() => handleDelete(item.id)}/>
                                {/*<Link to={`/edit/${item.id}`} className="text-black-500 hover:text-purple-700">*/}
                                {/*    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"*/}
                                {/*         stroke-width="1.5" stroke="currentColor" className="size-5">*/}
                                {/*        <path stroke-linecap="round" stroke-linejoin="round"*/}
                                {/*              d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L6.832 19.82a4.5 4.5 0 0 1-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 0 1 1.13-1.897L16.863 4.487Zm0 0L19.5 7.125"/>*/}
                                {/*    </svg>*/}
                                {/*</Link>*/}
                            </div>
                        </div>

                    </div>
                    )}
            </div>
        </>
    );
}

export default ProductListPage;