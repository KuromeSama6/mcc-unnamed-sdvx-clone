# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 3.22

# Delete rule output on recipe failure.
.DELETE_ON_ERROR:

#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:

# Disable VCS-based implicit rules.
% : %,v

# Disable VCS-based implicit rules.
% : RCS/%

# Disable VCS-based implicit rules.
% : RCS/%,v

# Disable VCS-based implicit rules.
% : SCCS/s.%

# Disable VCS-based implicit rules.
% : s.%

.SUFFIXES: .hpux_make_needs_suffix_list

# Command-line flag to silence nested $(MAKE).
$(VERBOSE)MAKESILENT = -s

#Suppress display of executed commands.
$(VERBOSE).SILENT:

# A target that is always out of date.
cmake_force:
.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /usr/bin/cmake

# The command to remove a file.
RM = /usr/bin/cmake -E rm -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = /home/playtest/dev/mcc-unnamed-sdvx-clone

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = /home/playtest/dev/mcc-unnamed-sdvx-clone

# Include any dependencies generated for this target.
include Graphics/CMakeFiles/Graphics.dir/depend.make
# Include any dependencies generated by the compiler for this target.
include Graphics/CMakeFiles/Graphics.dir/compiler_depend.make

# Include the progress variables for this target.
include Graphics/CMakeFiles/Graphics.dir/progress.make

# Include the compile flags for this target's objects.
include Graphics/CMakeFiles/Graphics.dir/flags.make

Graphics/CMakeFiles/Graphics.dir/src/Font.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/Font.cpp.o: Graphics/src/Font.cpp
Graphics/CMakeFiles/Graphics.dir/src/Font.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_1) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/Font.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/Font.cpp.o -MF CMakeFiles/Graphics.dir/src/Font.cpp.o.d -o CMakeFiles/Graphics.dir/src/Font.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Font.cpp

Graphics/CMakeFiles/Graphics.dir/src/Font.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/Font.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Font.cpp > CMakeFiles/Graphics.dir/src/Font.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/Font.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/Font.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Font.cpp -o CMakeFiles/Graphics.dir/src/Font.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.o: Graphics/src/Gamepad_Impl.cpp
Graphics/CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_2) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.o -MF CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.o.d -o CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Gamepad_Impl.cpp

Graphics/CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Gamepad_Impl.cpp > CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Gamepad_Impl.cpp -o CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/Image.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/Image.cpp.o: Graphics/src/Image.cpp
Graphics/CMakeFiles/Graphics.dir/src/Image.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_3) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/Image.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/Image.cpp.o -MF CMakeFiles/Graphics.dir/src/Image.cpp.o.d -o CMakeFiles/Graphics.dir/src/Image.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Image.cpp

Graphics/CMakeFiles/Graphics.dir/src/Image.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/Image.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Image.cpp > CMakeFiles/Graphics.dir/src/Image.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/Image.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/Image.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Image.cpp -o CMakeFiles/Graphics.dir/src/Image.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/ImageLoader.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/ImageLoader.cpp.o: Graphics/src/ImageLoader.cpp
Graphics/CMakeFiles/Graphics.dir/src/ImageLoader.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_4) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/ImageLoader.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/ImageLoader.cpp.o -MF CMakeFiles/Graphics.dir/src/ImageLoader.cpp.o.d -o CMakeFiles/Graphics.dir/src/ImageLoader.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/ImageLoader.cpp

Graphics/CMakeFiles/Graphics.dir/src/ImageLoader.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/ImageLoader.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/ImageLoader.cpp > CMakeFiles/Graphics.dir/src/ImageLoader.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/ImageLoader.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/ImageLoader.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/ImageLoader.cpp -o CMakeFiles/Graphics.dir/src/ImageLoader.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/Material.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/Material.cpp.o: Graphics/src/Material.cpp
Graphics/CMakeFiles/Graphics.dir/src/Material.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_5) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/Material.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/Material.cpp.o -MF CMakeFiles/Graphics.dir/src/Material.cpp.o.d -o CMakeFiles/Graphics.dir/src/Material.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Material.cpp

Graphics/CMakeFiles/Graphics.dir/src/Material.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/Material.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Material.cpp > CMakeFiles/Graphics.dir/src/Material.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/Material.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/Material.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Material.cpp -o CMakeFiles/Graphics.dir/src/Material.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/Mesh.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/Mesh.cpp.o: Graphics/src/Mesh.cpp
Graphics/CMakeFiles/Graphics.dir/src/Mesh.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_6) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/Mesh.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/Mesh.cpp.o -MF CMakeFiles/Graphics.dir/src/Mesh.cpp.o.d -o CMakeFiles/Graphics.dir/src/Mesh.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Mesh.cpp

Graphics/CMakeFiles/Graphics.dir/src/Mesh.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/Mesh.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Mesh.cpp > CMakeFiles/Graphics.dir/src/Mesh.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/Mesh.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/Mesh.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Mesh.cpp -o CMakeFiles/Graphics.dir/src/Mesh.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.o: Graphics/src/MeshGenerators.cpp
Graphics/CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_7) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.o -MF CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.o.d -o CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/MeshGenerators.cpp

Graphics/CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/MeshGenerators.cpp > CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/MeshGenerators.cpp -o CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/OpenGL.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/OpenGL.cpp.o: Graphics/src/OpenGL.cpp
Graphics/CMakeFiles/Graphics.dir/src/OpenGL.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_8) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/OpenGL.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/OpenGL.cpp.o -MF CMakeFiles/Graphics.dir/src/OpenGL.cpp.o.d -o CMakeFiles/Graphics.dir/src/OpenGL.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/OpenGL.cpp

Graphics/CMakeFiles/Graphics.dir/src/OpenGL.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/OpenGL.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/OpenGL.cpp > CMakeFiles/Graphics.dir/src/OpenGL.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/OpenGL.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/OpenGL.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/OpenGL.cpp -o CMakeFiles/Graphics.dir/src/OpenGL.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.o: Graphics/src/ParticleSystem.cpp
Graphics/CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_9) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.o -MF CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.o.d -o CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/ParticleSystem.cpp

Graphics/CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/ParticleSystem.cpp > CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/ParticleSystem.cpp -o CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/RenderQueue.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/RenderQueue.cpp.o: Graphics/src/RenderQueue.cpp
Graphics/CMakeFiles/Graphics.dir/src/RenderQueue.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_10) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/RenderQueue.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/RenderQueue.cpp.o -MF CMakeFiles/Graphics.dir/src/RenderQueue.cpp.o.d -o CMakeFiles/Graphics.dir/src/RenderQueue.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/RenderQueue.cpp

Graphics/CMakeFiles/Graphics.dir/src/RenderQueue.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/RenderQueue.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/RenderQueue.cpp > CMakeFiles/Graphics.dir/src/RenderQueue.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/RenderQueue.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/RenderQueue.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/RenderQueue.cpp -o CMakeFiles/Graphics.dir/src/RenderQueue.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.o: Graphics/src/ResourceManagers.cpp
Graphics/CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_11) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.o -MF CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.o.d -o CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/ResourceManagers.cpp

Graphics/CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/ResourceManagers.cpp > CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/ResourceManagers.cpp -o CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/Shader.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/Shader.cpp.o: Graphics/src/Shader.cpp
Graphics/CMakeFiles/Graphics.dir/src/Shader.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_12) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/Shader.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/Shader.cpp.o -MF CMakeFiles/Graphics.dir/src/Shader.cpp.o.d -o CMakeFiles/Graphics.dir/src/Shader.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Shader.cpp

Graphics/CMakeFiles/Graphics.dir/src/Shader.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/Shader.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Shader.cpp > CMakeFiles/Graphics.dir/src/Shader.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/Shader.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/Shader.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Shader.cpp -o CMakeFiles/Graphics.dir/src/Shader.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/SpriteMap.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/SpriteMap.cpp.o: Graphics/src/SpriteMap.cpp
Graphics/CMakeFiles/Graphics.dir/src/SpriteMap.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_13) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/SpriteMap.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/SpriteMap.cpp.o -MF CMakeFiles/Graphics.dir/src/SpriteMap.cpp.o.d -o CMakeFiles/Graphics.dir/src/SpriteMap.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/SpriteMap.cpp

Graphics/CMakeFiles/Graphics.dir/src/SpriteMap.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/SpriteMap.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/SpriteMap.cpp > CMakeFiles/Graphics.dir/src/SpriteMap.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/SpriteMap.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/SpriteMap.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/SpriteMap.cpp -o CMakeFiles/Graphics.dir/src/SpriteMap.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/Texture.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/Texture.cpp.o: Graphics/src/Texture.cpp
Graphics/CMakeFiles/Graphics.dir/src/Texture.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_14) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/Texture.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/Texture.cpp.o -MF CMakeFiles/Graphics.dir/src/Texture.cpp.o.d -o CMakeFiles/Graphics.dir/src/Texture.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Texture.cpp

Graphics/CMakeFiles/Graphics.dir/src/Texture.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/Texture.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Texture.cpp > CMakeFiles/Graphics.dir/src/Texture.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/Texture.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/Texture.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Texture.cpp -o CMakeFiles/Graphics.dir/src/Texture.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/VertexFormat.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/VertexFormat.cpp.o: Graphics/src/VertexFormat.cpp
Graphics/CMakeFiles/Graphics.dir/src/VertexFormat.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_15) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/VertexFormat.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/VertexFormat.cpp.o -MF CMakeFiles/Graphics.dir/src/VertexFormat.cpp.o.d -o CMakeFiles/Graphics.dir/src/VertexFormat.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/VertexFormat.cpp

Graphics/CMakeFiles/Graphics.dir/src/VertexFormat.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/VertexFormat.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/VertexFormat.cpp > CMakeFiles/Graphics.dir/src/VertexFormat.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/VertexFormat.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/VertexFormat.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/VertexFormat.cpp -o CMakeFiles/Graphics.dir/src/VertexFormat.cpp.s

Graphics/CMakeFiles/Graphics.dir/src/Window.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/src/Window.cpp.o: Graphics/src/Window.cpp
Graphics/CMakeFiles/Graphics.dir/src/Window.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_16) "Building CXX object Graphics/CMakeFiles/Graphics.dir/src/Window.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/src/Window.cpp.o -MF CMakeFiles/Graphics.dir/src/Window.cpp.o.d -o CMakeFiles/Graphics.dir/src/Window.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Window.cpp

Graphics/CMakeFiles/Graphics.dir/src/Window.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/src/Window.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Window.cpp > CMakeFiles/Graphics.dir/src/Window.cpp.i

Graphics/CMakeFiles/Graphics.dir/src/Window.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/src/Window.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/src/Window.cpp -o CMakeFiles/Graphics.dir/src/Window.cpp.s

Graphics/CMakeFiles/Graphics.dir/stdafx.cpp.o: Graphics/CMakeFiles/Graphics.dir/flags.make
Graphics/CMakeFiles/Graphics.dir/stdafx.cpp.o: Graphics/stdafx.cpp
Graphics/CMakeFiles/Graphics.dir/stdafx.cpp.o: Graphics/CMakeFiles/Graphics.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_17) "Building CXX object Graphics/CMakeFiles/Graphics.dir/stdafx.cpp.o"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT Graphics/CMakeFiles/Graphics.dir/stdafx.cpp.o -MF CMakeFiles/Graphics.dir/stdafx.cpp.o.d -o CMakeFiles/Graphics.dir/stdafx.cpp.o -c /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/stdafx.cpp

Graphics/CMakeFiles/Graphics.dir/stdafx.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/Graphics.dir/stdafx.cpp.i"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/stdafx.cpp > CMakeFiles/Graphics.dir/stdafx.cpp.i

Graphics/CMakeFiles/Graphics.dir/stdafx.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/Graphics.dir/stdafx.cpp.s"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/stdafx.cpp -o CMakeFiles/Graphics.dir/stdafx.cpp.s

# Object files for target Graphics
Graphics_OBJECTS = \
"CMakeFiles/Graphics.dir/src/Font.cpp.o" \
"CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.o" \
"CMakeFiles/Graphics.dir/src/Image.cpp.o" \
"CMakeFiles/Graphics.dir/src/ImageLoader.cpp.o" \
"CMakeFiles/Graphics.dir/src/Material.cpp.o" \
"CMakeFiles/Graphics.dir/src/Mesh.cpp.o" \
"CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.o" \
"CMakeFiles/Graphics.dir/src/OpenGL.cpp.o" \
"CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.o" \
"CMakeFiles/Graphics.dir/src/RenderQueue.cpp.o" \
"CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.o" \
"CMakeFiles/Graphics.dir/src/Shader.cpp.o" \
"CMakeFiles/Graphics.dir/src/SpriteMap.cpp.o" \
"CMakeFiles/Graphics.dir/src/Texture.cpp.o" \
"CMakeFiles/Graphics.dir/src/VertexFormat.cpp.o" \
"CMakeFiles/Graphics.dir/src/Window.cpp.o" \
"CMakeFiles/Graphics.dir/stdafx.cpp.o"

# External object files for target Graphics
Graphics_EXTERNAL_OBJECTS =

lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/Font.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/Gamepad_Impl.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/Image.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/ImageLoader.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/Material.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/Mesh.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/MeshGenerators.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/OpenGL.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/ParticleSystem.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/RenderQueue.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/ResourceManagers.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/Shader.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/SpriteMap.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/Texture.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/VertexFormat.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/src/Window.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/stdafx.cpp.o
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/build.make
lib/libGraphics_Release.a: Graphics/CMakeFiles/Graphics.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --bold --progress-dir=/home/playtest/dev/mcc-unnamed-sdvx-clone/CMakeFiles --progress-num=$(CMAKE_PROGRESS_18) "Linking CXX static library ../lib/libGraphics_Release.a"
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && $(CMAKE_COMMAND) -P CMakeFiles/Graphics.dir/cmake_clean_target.cmake
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && $(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/Graphics.dir/link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
Graphics/CMakeFiles/Graphics.dir/build: lib/libGraphics_Release.a
.PHONY : Graphics/CMakeFiles/Graphics.dir/build

Graphics/CMakeFiles/Graphics.dir/clean:
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics && $(CMAKE_COMMAND) -P CMakeFiles/Graphics.dir/cmake_clean.cmake
.PHONY : Graphics/CMakeFiles/Graphics.dir/clean

Graphics/CMakeFiles/Graphics.dir/depend:
	cd /home/playtest/dev/mcc-unnamed-sdvx-clone && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /home/playtest/dev/mcc-unnamed-sdvx-clone /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics /home/playtest/dev/mcc-unnamed-sdvx-clone /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics /home/playtest/dev/mcc-unnamed-sdvx-clone/Graphics/CMakeFiles/Graphics.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : Graphics/CMakeFiles/Graphics.dir/depend

