# 🚀 Shindo Launcher

O **Shindo Launcher** é o launcher oficial do **Shindo Client**, desenvolvido para oferecer uma experiência de jogo simples, prática e totalmente integrada.

Com ele, você pode baixar, atualizar e iniciar o **Shindo Client** com facilidade, além de personalizar configurações como a quantidade de RAM utilizada pelo Minecraft.

---

## ✨ Recursos Principais
- ✅ **Atualizações automáticas** – O launcher verifica e aplica updates do Shindo Client de forma rápida.  
- ✅ **Configuração de RAM** – Ajuste a memória alocada para o Minecraft com um slider intuitivo.  
- ✅ **Detecção de RAM do sistema** – O launcher detecta a RAM do seu PC e define limites seguros.  
- ✅ **Sistema de configuração em `.ini`** – Suas preferências de RAM ficam salvas na pasta `.shindo`.  
- ✅ **Integração com CmlLib.Core** – Início do jogo otimizado, direto pelo launcher.  
- ✅ **Compatível com Windows e Linux** – Execute o launcher tanto no Windows quanto no Linux.

---

## 📥 Instalação

### 🔵 Windows
1. Baixe a versão mais recente do launcher:  
   👉 [**Shindo Launcher – Releases**](https://github.com/ShindoClient/Shindo-Launcher/releases)  
2. Execute o instalador (`.exe`) e siga as instruções.  
3. (Opcional) Marque a opção **“Iniciar o Shindo Client”** no final da instalação para abrir o jogo imediatamente.

### 🟢 Linux
1. Baixe o arquivo **AppImage** na aba de releases:  
   👉 [**Shindo Launcher – Releases**](https://github.com/ShindoClient/Shindo-Launcher/releases)  
2. Dê permissão de execução:
   ```bash
   chmod +x Shindo_Launcher-x86_64.AppImage
   ```
3. Execute o launcher:
   ```bash
   ./Shindo_Launcher-x86_64.AppImage
   ```
4. Na primeira execução, os arquivos necessários serão extraídos para:
   ```
   ~/.config/.shindo
   ```

---

## ⚙️ Requisitos

### 🔵 Windows
- Windows 10 ou superior  
- .NET 8 Runtime instalado ([download aqui](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))  
- Java incluído no próprio `.shindo` (não precisa instalar manualmente)

### 🟢 Linux
- Distribuição com suporte a .NET 8 (Ubuntu 20.04+, Debian 11+, Fedora 36+)  
- **Dependências nativas para JavaFX e Avalonia**  
  ```bash
  sudo apt update
  sudo apt install -y     libgtk-3-0 libgtk-3-dev libxtst6 libxslt1.1 libxxf86vm1     libcanberra-gtk-module libglib2.0-0     libgdk-pixbuf2.0-0 libpangocairo-1.0-0 libatk1.0-0 libatk-bridge2.0-0     libx11-6 libxext6 libxrandr2 libxrender1 libxi6     libasound2 libpulse0 libdbus-glib-1-2 libnss3 libfontconfig1     libjpeg-turbo8 libfreetype6
  ```  
- Java incluído no próprio `.shindo` (não precisa instalar manualmente)

---

## 🎮 Como usar
1. Abra o **Shindo Launcher**.  
2. Clique em **INICIAR** para jogar.  
3. (Opcional) Clique em **OPÇÕES** para:  
   - Ajustar a quantidade de RAM dedicada ao Minecraft.  
   - Salvar automaticamente a configuração no `launcher.ini`.

---

## 🛠️ Contribuindo
Pull Requests são bem-vindos!  
Se você quiser sugerir melhorias ou reportar problemas, crie uma **issue**:  
👉 [**Abrir Issue**](https://github.com/ShindoClient/Shindo-Launcher/issues)

---

## 📜 Licença
Este projeto é licenciado sob a [GPL v3](https://www.gnu.org/licenses/gpl-3.0.html).  
Você é livre para usar, modificar e distribuir, desde que mantenha os créditos e a licença.

---

## 📡 Links úteis
🌐 **Site Oficial:** https://shindoclient.github.io  
💬 **Discord:** https://shindoclient.github.io/discord
