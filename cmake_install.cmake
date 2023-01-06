# Install script for directory: /home/playtest/dev/mcc-unnamed-sdvx-clone

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "/usr/local")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "Release")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Install shared libraries without execute permission?
if(NOT DEFINED CMAKE_INSTALL_SO_NO_EXE)
  set(CMAKE_INSTALL_SO_NO_EXE "1")
endif()

# Is this installation the result of a crosscompile?
if(NOT DEFINED CMAKE_CROSSCOMPILING)
  set(CMAKE_CROSSCOMPILING "FALSE")
endif()

# Set default install directory permissions.
if(NOT DEFINED CMAKE_OBJDUMP)
  set(CMAKE_OBJDUMP "/usr/bin/objdump")
endif()

if(NOT CMAKE_INSTALL_LOCAL_ONLY)
  # Include the install script for each subdirectory.
  include("/home/playtest/dev/mcc-unnamed-sdvx-clone/third_party/cmake_install.cmake")
  include("/home/playtest/dev/mcc-unnamed-sdvx-clone/Shared/cmake_install.cmake")
  include("/home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/cmake_install.cmake")
  include("/home/playtest/dev/mcc-unnamed-sdvx-clone/Main/cmake_install.cmake")
  include("/home/playtest/dev/mcc-unnamed-sdvx-clone/Audio/cmake_install.cmake")
  include("/home/playtest/dev/mcc-unnamed-sdvx-clone/Beatmap/cmake_install.cmake")
  include("/home/playtest/dev/mcc-unnamed-sdvx-clone/GUI/cmake_install.cmake")
  include("/home/playtest/dev/mcc-unnamed-sdvx-clone/Tests/cmake_install.cmake")
  include("/home/playtest/dev/mcc-unnamed-sdvx-clone/Tests.Shared/cmake_install.cmake")
  include("/home/playtest/dev/mcc-unnamed-sdvx-clone/Tests.Game/cmake_install.cmake")

endif()

if(CMAKE_INSTALL_COMPONENT)
  set(CMAKE_INSTALL_MANIFEST "install_manifest_${CMAKE_INSTALL_COMPONENT}.txt")
else()
  set(CMAKE_INSTALL_MANIFEST "install_manifest.txt")
endif()

string(REPLACE ";" "\n" CMAKE_INSTALL_MANIFEST_CONTENT
       "${CMAKE_INSTALL_MANIFEST_FILES}")
file(WRITE "/home/playtest/dev/mcc-unnamed-sdvx-clone/${CMAKE_INSTALL_MANIFEST}"
     "${CMAKE_INSTALL_MANIFEST_CONTENT}")
