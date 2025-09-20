#include "BoostSerializeFuncionario.h"
#include <boost/asio.hpp>
using namespace boost::asio;
using ip::tcp;
using std::string;
using std::cout;
using std::endl;

string jRead(tcp::socket & socket) {
       boost::system::error_code error;
       boost::asio::streambuf buffer;
       auto bytes=boost::asio::read(socket, buffer, boost::asio::transfer_all(),error);
       ostream oss(&buffer);
       std::stringstream ss;
       ss<<oss.rdbuf();
       std::string str_data = ss.str();
       cout<<"received "<<bytes<<" bytes"<<endl;
       return str_data;
}
void jSend(tcp::socket & socket, const string& message) {
       const string msg = message + "\n";
       boost::asio::write( socket, boost::asio::buffer(message) );
}

int main() {
      boost::asio::io_service ioService;
      tcp::acceptor jListener(ioService, tcp::endpoint(tcp::v4(), 5003 )); //listen for new connection

      tcp::socket jSocket(ioService); //socket creation 
      jListener.accept(jSocket); //waiting for connection

      //read operation
      boost::system::error_code error;
      string mssg=jRead(jSocket);

      Funcionario funcionario;
      //Loading data in funcionario with binary archive.
      cout << "********************* Server Socket Received *********************" << endl;
      funcionario.load(mssg);
      funcionario.toStr();

      return 0;
}

