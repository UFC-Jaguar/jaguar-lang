#include "BoostSerializeFuncionario.h"
#include <boost/asio.hpp>
using namespace boost::asio;
using ip::tcp;
using std::cout;
using std::endl;

void onSendCompleted(boost::system::error_code jError, size_t bytesTransferred) {
    if (jError){
        std::cout << "Send failed: " << jError.message() << endl;
        return;
    }
    std::cout << "Send succesful (" << bytesTransferred << " bytes)" << endl;
}

int main() {
     Funcionario joao("Joao da Silva", 40, "UFC", "RUSSAS");
     
     boost::asio::io_service ioService;
     tcp::socket socket(ioService); //socket creation
     socket.connect(tcp::endpoint(boost::asio::ip::address::from_string("127.0.0.1"), 5003 )); //connection

     boost::asio::streambuf jBuffer; //the jBuffer
     std::ostream jOStream(&jBuffer);

     joao.save(jOStream); //saving data in jOStream

     boost::asio::async_write(socket, jBuffer, onSendCompleted);

     return 0;
}

//#include <boost/function.hpp>
//#include <boost/bind/bind.hpp>
//#include <functional>
//boost::system::error_code error;

